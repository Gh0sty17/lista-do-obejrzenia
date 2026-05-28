using DoObejrzenia;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace DoObejrzenia

{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Movie> Movies { get; set; }
        public ICollectionView MoviesView { get; set; }

        private string _newTitle;
        public string NewTitle
        {
            get => _newTitle;
            set { _newTitle = value; OnPropertyChanged(nameof(NewTitle)); }
        }

        private string _newGenre;
        public string NewGenre
        {
            get => _newGenre;
            set { _newGenre = value; OnPropertyChanged(nameof(NewGenre)); }
        }

        private string _newYear;
        public string NewYear
        {
            get => _newYear;
            set { _newYear = value; OnPropertyChanged(nameof(NewYear)); }
        }

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                MoviesView.Refresh();
            }
        }

        private Movie _selectedMovie;
        public Movie SelectedMovie
        {
            get => _selectedMovie;
            set { _selectedMovie = value; OnPropertyChanged(nameof(SelectedMovie)); }
        }

        public ICommand AddMovieCommand { get; }
        public ICommand DeleteMovieCommand { get; }

        public MainViewModel()
        {
            Movies = new ObservableCollection<Movie>
            {
                new Movie { Title = "Incepcja", Genre = "Sci-Fi", Year = 2010 },
                new Movie { Title = "Gladiator", Genre = "Dramat", Year = 2000 }
            };

            MoviesView = CollectionViewSource.GetDefaultView(Movies);
            MoviesView.Filter = FilterMovies;

            AddMovieCommand = new RelayCommand(AddMovie, CanAddMovie);
            DeleteMovieCommand = new RelayCommand(DeleteMovie, CanDeleteMovie);
        }

        private bool FilterMovies(object obj)
        {
            if (string.IsNullOrEmpty(FilterText)) return true;
            if (obj is Movie movie)
            {
                return movie.Title.ToLower().Contains(FilterText.ToLower()) ||
                       movie.Genre.ToLower().Contains(FilterText.ToLower());
            }
            return false;
        }

        private void AddMovie(object param)
        {
            if (int.TryParse(NewYear, out int year))
            {
                Movies.Add(new Movie { Title = NewTitle, Genre = NewGenre, Year = year });
                NewTitle = string.Empty;
                NewGenre = string.Empty;
                NewYear = string.Empty;
            }
        }

        private bool CanAddMovie(object param) => !string.IsNullOrWhiteSpace(NewTitle);

        private void DeleteMovie(object param)
        {
            if (SelectedMovie != null)
            {
                Movies.Remove(SelectedMovie);
            }
        }

        private bool CanDeleteMovie(object param) => SelectedMovie != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}