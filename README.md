# 📦 Magazyn_WPF
# Nazwa zespołu: Magazynierzy

Desktopowa aplikacja WPF do zarządzania stanem magazynowym.

---

## 👥 Skład zespołu

- Maciej Olędzki  
- Krzysztof Carewicz  
- Julia Żukowska  
- Rafał Ciereszko  

---

## 🎯 Wybrany temat

**Magazyn produktów** – aplikacja desktopowa WPF umożliwiająca zarządzanie stanem magazynu.

---

## 📝 Opis projektu

Celem projektu jest stworzenie aplikacji pozwalającej na zarządzanie produktami w magazynie.  
Użytkownik może:

- dodawać nowe produkty
- edytować istniejące produkty
- usuwać produkty
- przeglądać aktualny stan magazynu

System umożliwia kontrolę ilości produktów oraz zapobiega wprowadzaniu błędnych danych dzięki walidacji i podstawowej logice biznesowej.

---

## 🗂 Model danych (Encje)

### Produkt
- Id
- Nazwa
- Kategoria
- Ilość
- Jednostka
- Lokalizacja
- Data dodania

### Kategoria
- Id
- Nazwa
- Opis

---

## ✅ Minimalny zakres funkcjonalności (MVP)

- Wyświetlanie listy produktów (Data Binding)
- Dodawanie, edycja i usuwanie produktów (CRUD)
- Zapisywanie i wczytywanie danych z pliku (trwałość danych)
- Walidacja danych (np. ilość ≥ 0, wymagana nazwa)
- Obsługa podstawowych błędów użytkownika
- Interfejs użytkownika w WPF (XAML)
- Wykorzystanie Commands

---

## 🚀 Potencjalne rozszerzenia

- 🔎 Filtrowanie i wyszukiwanie produktów (np. po nazwie, kategorii)
- ↕️ Sortowanie według różnych kryteriów
- 🔗 Relacja produkt–kategoria
- 💾 Zapis danych w bazie SQLite
- 🧩 Zastosowanie wzorca MVVM
- 📤 Import / eksport danych (np. CSV)

---

## 🔗 Repozytorium

👉 https://github.com/julson00x/Magazyn_WPF
