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
        public string imiePubliczne
        {
            get{ return imie; }
        }
        public string nazwiskoPubliczne
        {
            get{ return nazwisko; }
        }
        public double rankingPubliczny
        {
            get{ return ranking; }
            set { ranking = value; }
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
        public static Mecz operator +(Gracz gracz1, Gracz gracz2)
        {
            return new Mecz(gracz1, gracz2);
        }
    }
    class Mecz
    {
        private Gracz gracz1, gracz2;
        public Mecz(Gracz gracz1, Gracz gracz2)
        {
            this.gracz1 = gracz1;
            this.gracz2 = gracz2;
        }
        public void RozegrajMecz(int wynik)
        {
            double k = 32;
            double E1 = 1 / (1 + Math.Pow(10, (gracz2.rankingPubliczny - gracz1.rankingPubliczny) / 400));
            double E2 = 1 / (1 + Math.Pow(10, (gracz1.rankingPubliczny - gracz2.rankingPubliczny) / 400));
            double S1 = 0;
            double S2 = 0;
            if (wynik == 1)
            {
                S1 = 1;
                S2 = 0;
            }
            else if (wynik == 2)
            {
                S1 = 0;
                S2 = 1;
            }
            gracz1.rankingPubliczny = gracz1.rankingPubliczny + k * (S1 - E1);
            gracz2.rankingPubliczny = gracz2.rankingPubliczny + k * (S2 - E2);
        }
    }
    class Program
    {
        static void Main()
        {
            List<Gracz> Gracze = new List<Gracz>();
            string path = @"gracze.txt";
            bool loop = true;
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] dane = line.Split(';');
                        Gracze.Add(new Gracz(dane[0], dane[1], dane[2], double.Parse(dane[3])));
                    }
                    sr.Dispose();
                }
            }
            else
            {
                File.Create(path).Dispose();
            }
            while (loop)
            {
                Console.Clear();
                Console.WriteLine("----> System Rankingowy <---");
                Console.WriteLine("1. Gracze");
                Console.WriteLine("2. Mecze");
                Console.WriteLine("3. Top 10");
                Console.WriteLine("4. ZAPISZ ZMIANY");
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
                        Console.WriteLine("4. Wyswietl graczy");
                        Console.WriteLine("5. Wyświetl dokładne dane gracza");
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
                                Console.WriteLine("Wpisz nick gracza ktorego chcesz edytowac ranking: ");
                                string nickEdytuj = Console.ReadLine();
                                for (int i = 0; i < Gracze.Count; i++)
                                {
                                    if (Gracze[i].nickPubliczny == nickEdytuj)
                                    {
                                        Console.WriteLine("Podaj nowy ranking dla gracza {0}: ", Gracze[i].nickPubliczny);
                                        string rankingStringEdytuj = Console.ReadLine();
                                        double rankingEdytuj;
                                        if (!double.TryParse(rankingStringEdytuj, out rankingEdytuj))
                                        {
                                            Console.WriteLine("BLAD! Ranking musi byc liczba.");
                                            Console.ReadKey();
                                            Console.Clear();
                                            break;
                                        }
                                        Console.WriteLine("Czy na pewno chcesz zmienic ranking gracza {0} na {1}?(Y/N)", Gracze[i].nickPubliczny, rankingEdytuj);
                                        char potwierdzenieEdytuj = Console.ReadKey().KeyChar;
                                        if (potwierdzenieEdytuj == 'Y' || potwierdzenieEdytuj == 'y')
                                        {
                                            Gracze[i] = new Gracz(Gracze[i].imiePubliczne, Gracze[i].nazwiskoPubliczne, Gracze[i].nickPubliczny, rankingEdytuj);
                                            Console.WriteLine("Pomyślnie zmieniono ranking gracza.");
                                            Console.ReadKey();
                                        }
                                        else
                                        {
                                            Console.WriteLine("Nie zmieniono ranking gracza.");
                                            Console.ReadKey();
                                        }
                                    }
                                }
                                Console.Clear();
                                break;
                            case '4':
                                Console.WriteLine("----> Wyswietl graczy <---");
                                foreach (Gracz gracz in Gracze)
                                {
                                    Console.WriteLine("{0} posiada ranking {1:F1} elo", gracz.nickPubliczny, gracz.rankingPubliczny);
                                }

                                Console.ReadKey();
                                Console.Clear();
                                break;
                            case '5':
                                Console.WriteLine("----> Wyswietl dokładne dane gracza <---");
                                Console.WriteLine("Wpisz nick gracza ktorego chcesz wyswietlic: ");
                                string nickWyswietl = Console.ReadLine();
                                for (int i = 0; i < Gracze.Count; i++)
                                {
                                    if (Gracze[i].nickPubliczny == nickWyswietl)
                                    {
                                        Gracze[i].WyswietlDane();
                                        Console.ReadKey();
                                    }
                                }
                                break;
                            case '0':
                                Console.Clear();
                                break;
                        }

                        break;
                    case '2':
                        Console.WriteLine("----> Mecze <---");
                        Console.WriteLine("1. Zagraj mecz");
                        opcja = Console.ReadKey().KeyChar;
                        switch (opcja)
                        {
                            case '1':
                            {
                                Console.WriteLine("----> Zagraj mecz <---");
                                Console.WriteLine("Wpisz nick gracza 1: ");
                                string nickGracz1 = Console.ReadLine();
                                Console.WriteLine("Wpisz nick gracza 2: ");
                                string nickGracz2 = Console.ReadLine();
                                Gracz gracz1 = Gracze.Find(gracz => gracz.nickPubliczny == nickGracz1);
                                Gracz gracz2 = Gracze.Find(gracz => gracz.nickPubliczny == nickGracz2);
                                if (gracz1 == null || gracz2 == null)
                                {
                                    Console.WriteLine("BLAD! Gracz nie istnieje.");
                                    Console.ReadKey();
                                    break;
                                }
                                Console.WriteLine("Gracz 1: {0} posiada ranking {1:F1} elo", gracz1.nickPubliczny, gracz1.rankingPubliczny);
                                Console.WriteLine("Gracz 2: {0} posiada ranking {1:F1} elo", gracz2.nickPubliczny, gracz2.rankingPubliczny);
                                double StaryRankingGracz1 = gracz1.rankingPubliczny;
                                double StaryRankingGracz2 = gracz2.rankingPubliczny;
                                Console.WriteLine("Kto wygral mecz?");
                                Console.WriteLine("1. {0}", gracz1.nickPubliczny);
                                Console.WriteLine("2. {0}", gracz2.nickPubliczny);
                                char wynik = Console.ReadKey().KeyChar;
                                if (wynik != '1' && wynik != '2')
                                {
                                    Console.WriteLine("BLAD! Wybierz 1 lub 2.");
                                    Console.ReadKey();
                                    break;
                                }
                                Mecz mecz = gracz1 + gracz2;
                                mecz.RozegrajMecz(int.Parse(wynik.ToString()));
                                Console.WriteLine("Pomyślnie rozegrano mecz.");
                                Console.WriteLine("Nowy ranking {0} wynosi {1:F1} elo, zmiana o {2:F1} punktów", gracz1.nickPubliczny, gracz1.rankingPubliczny, gracz1.rankingPubliczny - StaryRankingGracz1);
                                Console.WriteLine("Nowy ranking {0} wynosi {1:F1} elo, zmiana o {2:F1} punktów", gracz2.nickPubliczny, gracz2.rankingPubliczny, gracz2.rankingPubliczny-StaryRankingGracz2);
                                Console.ReadKey();
                                break;
                            }
                        }
                        break;
                    case '3':
                        Console.WriteLine("OBECNE TOP 10");
                        List<Gracz> posortowaniGracze = Gracze.OrderByDescending(gracz => gracz.rankingPubliczny).ToList();
                        List<Gracz> top10Graczy = posortowaniGracze.Take(10).ToList();
                        foreach (Gracz gracz in top10Graczy)
                        {
                            Console.WriteLine("{0} posiada ranking {1:F1} elo", gracz.nickPubliczny, gracz.rankingPubliczny);
                        }
                        Console.ReadKey();
                        break;
                    case '4':
                    {
                        // Usuń istniejący plik
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }

                        // Utwórz nowy plik i zapisz do niego dane
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            foreach (Gracz gracz in Gracze)
                            {
                                sw.WriteLine("{0};{1};{2};{3}", gracz.imiePubliczne, gracz.nazwiskoPubliczne, gracz.nickPubliczny, gracz.rankingPubliczny);
                            }
                        }
                        Console.WriteLine("Pomyślnie zapisano zmiany.");
                        Console.ReadKey();
                    }
                        break;
                }
            }
        }
    }
}
