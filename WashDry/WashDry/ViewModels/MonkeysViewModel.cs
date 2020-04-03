using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace WashDry.ViewModels
{
    public class MonkeysViewModel : INotifyPropertyChanged
    {

        readonly IList<Monkey> source;

        public ObservableCollection<Monkey> Monkeys { get; private set; }
        public IList<Monkey> EmptyMonkeys { get; private set; }

        public Monkey PreviousMonkey { get; set; }
        public Monkey CurrentMonkey { get; set; }
        public Monkey CurrentItem { get; set; }
        public int PreviousPosition { get; set; }
        public int CurrentPosition { get; set; }
        public int Position { get; set; }

        public ICommand FilterCommand => new Command<string>(FilterItems);
        public ICommand ItemChangedCommand => new Command<Monkey>(ItemChanged);
        public ICommand PositionChangedCommand => new Command<int>(PositionChanged);
        public ICommand DeleteCommand => new Command<Monkey>(RemoveMonkey);
        public ICommand FavoriteCommand => new Command<Monkey>(FavoriteMonkey);


        public MonkeysViewModel()
        {
            source = new List<Monkey>();
            CreateMonkeyCollection();

            CurrentItem = Monkeys.Skip(3).FirstOrDefault();
            OnPropertyChanged("CurrentItem");
            Position = 3;
            OnPropertyChanged("Position");
        }

        void CreateMonkeyCollection()
        {
            source.Add(new Monkey
            {
                Name = "WASH DRY APP",
                Location = " ",
                Details = "Somos una emprea consiente del medio ambiente principalmente al ahorro del agua" +
                " que ofrece lavado de autos a domicilio . Incluyendo comodidad ,seguridad y rapidez. Gerenado empleos en la ciudad.",
                ImageUrl = "iko.png"
            });

            source.Add(new Monkey
            {
                Name = "Programa Cero Contaminacion ",
                Location = "",
                Details = "No usamos mas de 70 litros por auto, ni productos nocivos para el ambiente, sin grupo electrogeno, sin contaminacion acustica, sin contaminacion de acuiferos.",
                ImageUrl = "iko.png"
            });

            source.Add(new Monkey
            {
                Name = "Como lo hacemos?",
                Location = "",
                Details = "Contamos con washers capacitados y equipamiento, que llegar hasta tu ubicacion  para brindarte el mejor servicio"+
                ", lo unico que tienes que hacer es agendar con nosotros , registrate y vive la experiencia.",
                ImageUrl = "iko.png"
            });

            Monkeys = new ObservableCollection<Monkey>(source);
        }

        void FilterItems(string filter)
        {
            var filteredItems = source.Where(monkey => monkey.Name.ToLower().Contains(filter.ToLower())).ToList();
            foreach (var monkey in source)
            {
                if (!filteredItems.Contains(monkey))
                {
                    Monkeys.Remove(monkey);
                }
                else
                {
                    if (!Monkeys.Contains(monkey))
                    {
                        Monkeys.Add(monkey);
                    }
                }
            }
        }

        void ItemChanged(Monkey item)
        {
            PreviousMonkey = CurrentMonkey;
            CurrentMonkey = item;
            OnPropertyChanged("PreviousMonkey");
            OnPropertyChanged("CurrentMonkey");
        }

        void PositionChanged(int position)
        {
            PreviousPosition = CurrentPosition;
            CurrentPosition = position;
            OnPropertyChanged("PreviousPosition");
            OnPropertyChanged("CurrentPosition");
        }

        void RemoveMonkey(Monkey monkey)
        {
            if (Monkeys.Contains(monkey))
            {
                Monkeys.Remove(monkey);
            }
        }

        void FavoriteMonkey(Monkey monkey)
        {
            monkey.IsFavorite = !monkey.IsFavorite;
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
