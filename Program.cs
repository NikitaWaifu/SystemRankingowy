namespace rankingi
{
    class Gracz
    {
        private string imie, nazwisko, nick;
        private double ranking;
        public string nickPubliczny
        {
            get{ return nick; }
        }
        public double rankingPubliczny
        {
            get{ return ranking; }
        }
        
        public Gracz(string imie, string nazwisko, string nick, double ranking)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.nick = nick;
            this.ranking = ranking;
        }
        public void WyswietlDane()
        {
            Console.WriteLine("Imie: {0}", imie);
            Console.WriteLine("Nazwisko: {0}", nazwisko);
            Console.WriteLine("Nick: {0}", nick);
            Console.WriteLine("Ranking: {0}", ranking);
        }
    }
    class Program
    {
        static void Main()
        {
            List<Gracz> Gracze = new List<Gracz>();
            bool loop = true;
            Gracze.Add(new Gracz("Jan", "Kowalski", "Kowal", 1000));
            Gracze.Add(new Gracz("Adam ", "Nowak", "Nowy", 1000));
            Gracze.Add(new Gracz("Piotr", "Kowal", "Kowalczyk", 1400));
            while (loop)
            {
                Console.WriteLine("----> System Rankingowy <---");
                Console.WriteLine("1. Gracze");
                Console.WriteLine("2. Mecze");
                Console.WriteLine("3. Top 10");
                Console.WriteLine("0. Wyjscie");
                Console.WriteLine("Wybierz opcje: \n");
                char opcja = Console.ReadKey().KeyChar;
                Console.Clear();
                switch (opcja)
                {
                    case '1':
                        Console.Clear();
                        Console.WriteLine("----> Gracze <---");
                        Console.WriteLine("1. Zarejestruj gracza");
                        Console.WriteLine("2. Usun gracza");
                        Console.WriteLine("3. Edytuj ranking gracza");
                        Console.WriteLine("4. Wyswietl gracza");
                        Console.WriteLine("0. Powrot");
                        Console.Write("Wybierz opcje: \n");
                        char opcjaGracze = Console.ReadKey().KeyChar;
                        Console.Clear();
                        switch (opcjaGracze)
                        {
                            case '1':
                                Console.WriteLine("----> Zarejestruj gracza <---");
                                Console.Write("Imie: ");
                                string imie = Console.ReadLine();
                                if (string.IsNullOrWhiteSpace(imie))
                                {
                                    Console.WriteLine("BLAD! Imie nie moze byc puste.");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                }
                                Console.Write("Nazwisko: ");
                                string nazwisko = Console.ReadLine();
                                if (string.IsNullOrWhiteSpace(nazwisko))
                                {
                                    Console.WriteLine("BLAD! Nazwisko nie moze byc puste.");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                }
                                Console.Write("Nick: ");
                                string nick = Console.ReadLine();
                                if (string.IsNullOrWhiteSpace(nick))
                                {
                                    nick = nazwisko;
                                }
                                Console.Write("Ranking (Podstawowy wynosi 1000): ");
                                string rankingString = Console.ReadLine();
                                double ranking;
                                if (!double.TryParse(rankingString, out ranking))
                                {
                                    ranking = 1000;
                                }
                                Console.WriteLine("Czy chcesz dodać gracza {0} '{1}' {2} z rankingiem {3}?(Y/N)", imie, nick, nazwisko, ranking); 
                                char potwierdzenie = Console.ReadKey().KeyChar;
                                if (potwierdzenie == 'Y' || potwierdzenie == 'y')
                                {
                                    Gracze.Add(new Gracz(imie, nazwisko, nick, ranking));
                                    Console.WriteLine("Pomyślnie zarejestrowano gracza.");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.WriteLine("Nie dodano gracza.");
                                    Console.ReadKey();
                                }
                                Console.Clear();
                                break;
                            case '2':
                                Console.WriteLine("----> Usun gracza <---");
                                Console.WriteLine("Wpisz nick gracza ktorego chcesz usunac: ");
                                string nickUsun = Console.ReadLine();
                                for (int i = 0; i < Gracze.Count; i++)
                                {
                                    if (Gracze[i].nickPubliczny == nickUsun)
                                    {
                                        Console.WriteLine("Czy na pewno chcesz usunac gracza {0}?(Y/N)", Gracze[i].nickPubliczny);
                                        char potwierdzenieUsun = Console.ReadKey().KeyChar;
                                        if (potwierdzenieUsun == 'Y' || potwierdzenieUsun == 'y')
                                        {
                                            Gracze.RemoveAt(i);
                                            Console.WriteLine("Pomyślnie usunięto gracza.");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nie usunieto gracza.");
                                            Console.ReadKey();
                                        }
                                    }
                                }
                                Console.Clear();
                                break;
                            case '3':
                                Console.WriteLine("----> Edytuj ranking gracza <---");
                                Console.Clear();
                                break;
                            case '4':
                                Console.WriteLine("----> Wyswietl graczy <---");
                                foreach (Gracz gracz in Gracze)
                                {
                                    Console.WriteLine("Nick: {0} posiada ranking {1} elo", gracz.nickPubliczny, gracz.rankingPubliczny);
                                }

                                Console.ReadKey();
                                Console.Clear();
                                break;
                            case '0':
                                Console.Clear();
                                break;
                        }
                        break;
                }
            }
        }
    }
}