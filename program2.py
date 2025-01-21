import os
import uuid

class Gracz:
    def __init__(self, nick, imie, nazwisko, mmr):
        self.nick = nick
        self.imie = imie
        self.nazwisko = nazwisko
        self.mmr = mmr

    def __str__(self):
        return f"Gracz {self.imie} {self.nazwisko} o nicku {self.nick} posiada {self.mmr} MMR"

    def to_file(self):
        return f"{self.nick};{self.imie};{self.nazwisko};{self.mmr}\n"

class Mecz:
    def __init__(self, gracz1, gracz2):
        self.id = str(uuid.uuid4())
        self.gracz1 = gracz1
        self.gracz2 = gracz2
        self.wynik = None

    def __str__(self):
        wynik = "Brak wyniku" if not self.wynik else ("Remis" if self.wynik == "0" else f"Wygrał gracz {self.wynik}")
        return (f"Mecz ID: {self.id}\n"
                f"Pomiędzy:\n"
                f"{self.gracz1}\n"
                f"oraz\n"
                f"{self.gracz2}\n"
                f"Wynik: {wynik}")

    def to_file(self):
        return f"{self.id};{self.gracz1.nick};{self.gracz2.nick};{self.wynik or 'Brak'}\n"

def oblicz_zmiane_mmr(mmr1, mmr2, wynik, k=32):
    szansa1 = 1 / (1 + 10 ** ((mmr2 - mmr1) / 400))
    szansa2 = 1 - szansa1

    if wynik == "1":
        zmiana1 = round(k * (1 - szansa1))
        zmiana2 = round(k * (0 - szansa2))
    elif wynik == "2":
        zmiana1 = round(k * (0 - szansa1))
        zmiana2 = round(k * (1 - szansa2))
    elif wynik == "0":
        zmiana1 = round(k * (0.5 - szansa1))
        zmiana2 = round(k * (0.5 - szansa2))
    else:
        zmiana1 = zmiana2 = 0

    return zmiana1, zmiana2

def wyswietl_potencjalne_zmiany(gracz1, gracz2):
    zmiana1_wygrana, zmiana2_przegrana = oblicz_zmiane_mmr(gracz1.mmr, gracz2.mmr, "1")
    zmiana1_przegrana, zmiana2_wygrana = oblicz_zmiane_mmr(gracz1.mmr, gracz2.mmr, "2")
    zmiana1_remis, zmiana2_remis = oblicz_zmiane_mmr(gracz1.mmr, gracz2.mmr, "0")

    print(f"Potencjalne zmiany MMR:")
    print(f"  Gracz {gracz1.nick} (wygrana/przegrana/remis): +{zmiana1_wygrana}/{zmiana1_przegrana}/{zmiana1_remis}")
    print(f"  Gracz {gracz2.nick} (wygrana/przegrana/remis): +{zmiana2_wygrana}/{zmiana2_przegrana}/{zmiana2_remis}")


def ZapiszPlik(gracze, mecze):
    with open("gracze.txt", "w") as plik:
        for gracz in gracze:
            plik.write(gracz.to_file())

    with open("mecze.txt", "w") as plik:
        for mecz in mecze:
            plik.write(mecz.to_file())

def OdczytajPlik():
    gracze = []
    mecze = []

    if os.path.exists("gracze.txt"):
        with open("gracze.txt", "r") as plik:
            for linia in plik:
                linia = linia.strip()
                dane = linia.split(";")
                gracze.append(Gracz(dane[0], dane[1], dane[2], int(dane[3]), dane[4] == "True"))

    if os.path.exists("mecze.txt"):
        with open("mecze.txt", "r") as plik:
            for linia in plik:
                linia = linia.strip()
                dane = linia.split(";")
                mecz = Mecz(None, None)
                mecz.id = dane[0]
                mecz.gracz1 = next((g for g in gracze if g.nick == dane[1]), None)
                mecz.gracz2 = next((g for g in gracze if g.nick == dane[2]), None)
                mecz.wynik = dane[3] if dane[3] != "Brak" else None
                mecze.append(mecz)

    return gracze, mecze

listagraczy, zakonczone_mecze = OdczytajPlik()
trwajace_mecze = []

while True:
    print("WYBIERZ OPCJE")
    print("1 - dodaj gracza")
    print("2 - sprawdź gracza")
    print("3 - sprawdź listę graczy")
    print("4 - rozpoczęcie meczu")
    print("5 - sprawdź zakończone mecze")
    print("6 - przypisz wynik do trwającego meczu")
    print("7 - zapisz dane i wyjdź")

    wybor = input("Wybierz opcję: ")
    if wybor == "1":
        nick = input("Podaj nick: ")
        imie = input("Podaj imię: ")
        nazwisko = input("Podaj nazwisko: ")
        mmr = int(input("Podaj mmr: "))
        gracz = Gracz(nick, imie, nazwisko, mmr)
        print("Czy dane się zgadzają? (tak/nie)")
        print(gracz)
        zgoda = input()
        if zgoda.lower() == "tak":
            listagraczy.append(gracz)
            ZapiszPlik(listagraczy, zakonczone_mecze)
        else:
            print("Spróbuj ponownie")

    elif wybor == "2":
        nick = input("Podaj nick: ")
        gracz = next((g for g in listagraczy if g.nick == nick), None)
        print(gracz if gracz else "Gracza o takim nicku nie znaleziono.")

    elif wybor == "3":
        if not listagraczy:
            print("Lista graczy jest pusta.")
        for gracz in listagraczy:
            print(gracz)
        input()
    elif wybor == "4":
        nick1 = input("Podaj nick pierwszego gracza: ")
        nick2 = input("Podaj nick drugiego gracza: ")
        gracz1 = next((g for g in listagraczy if g.nick == nick1), None)
        gracz2 = next((g for g in listagraczy if g.nick == nick2), None)

        if gracz1 and gracz2:
            mecz = Mecz(gracz1, gracz2)
            trwajace_mecze.append(mecz)
            print("Mecz rozpoczęty:")
            print(mecz)
            wyswietl_potencjalne_zmiany(gracz1, gracz2)
            print("1. Wpisz wynik meczu")
            print("2. Anuluj mecz")
            print("3. Powrót do menu")
            wybor = input("Wybierz opcję: ")
            if wybor == "1":
                wynik = input("Podaj wynik meczu (1 - wygrał gracz 1, 2 - wygrał gracz 2, 0 - remis): ")
                if wynik in ["1", "2", "0"]:
                    zmiana1, zmiana2 = oblicz_zmiane_mmr(
                        gracz1.mmr,
                        gracz2.mmr,
                        wynik
                    )
                    gracz1.mmr += zmiana1
                    gracz2.mmr += zmiana2
                    mecz.wynik = wynik

                    zakonczone_mecze.append(mecz)
                    trwajace_mecze.remove(mecz)

                    print("Wynik zapisany. Aktualne MMR graczy:")
                    print(gracz1)
                    print(gracz2)
                    ZapiszPlik(listagraczy, zakonczone_mecze)
                    input()
                else:
                    print("Nieprawidłowy wynik meczu. Spróbuj ponownie.")
                    input()
            elif wybor == "2":
                trwajace_mecze.remove(mecz)
                print("Mecz anulowany.")
                input()
            elif wybor == "3":
                continue


    elif wybor == "5":
        if not zakonczone_mecze:
            print("Brak zakończonych meczów.")
        for mecz in zakonczone_mecze:
            print(mecz)


    elif wybor == "6":
        if not trwajace_mecze:
            print("Brak trwających meczów.")
        else:
            print("Trwające mecze:")
            for mecz in trwajace_mecze:
                print(mecz)

            uuid_meczu = input("Wprowadź UUID meczu, do którego chcesz przypisać wynik: ").strip()
            wybrany_mecz = next((mecz for mecz in trwajace_mecze if mecz.id == uuid_meczu), None)

            if wybrany_mecz:
                print("Potencjalne zmiany MMR dla trwającego meczu:")
                wyswietl_potencjalne_zmiany(wybrany_mecz.gracz1, wybrany_mecz.gracz2)
                wynik = input("Podaj wynik meczu (1 - wygrał gracz 1, 2 - wygrał gracz 2, 0 - remis): ").strip()
                if wynik in ["1", "2", "0"]:
                    zmiana1, zmiana2 = oblicz_zmiane_mmr(
                        wybrany_mecz.gracz1.mmr,
                        wybrany_mecz.gracz2.mmr,
                        wynik
                    )
                    wybrany_mecz.gracz1.mmr += zmiana1
                    wybrany_mecz.gracz2.mmr += zmiana2
                    wybrany_mecz.wynik = wynik

                    zakonczone_mecze.append(wybrany_mecz)
                    trwajace_mecze.remove(wybrany_mecz)

                    print("Wynik zapisany. Aktualne MMR graczy:")
                    print(wybrany_mecz.gracz1)
                    print(wybrany_mecz.gracz2)
                    ZapiszPlik(listagraczy, zakonczone_mecze)
                    input()
                else:
                    print("Nieprawidłowy wynik meczu. Spróbuj ponownie.")
                    input()
            else:
                print("Nie znaleziono meczu o podanym UUID.")

    elif wybor == "7":
        ZapiszPlik(listagraczy, zakonczone_mecze)
        print("Dane zapisane. Do zobaczenia!")
        break
    else:
        print("Nieznana opcja, spróbuj ponownie.")
