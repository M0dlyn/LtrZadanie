# Order Calculator - Zadanie LTR Labs

## Wymagania

Aby uruchomić projekt potrzebujesz:
- **.NET 8.0 SDK**

Sprawdź wersję .NET:
```bash
dotnet --version
```

---

## Uruchomienie Projektu

### Uruchom Demo
```bash
dotnet run
```

### Uruchom Testy
```bash
dotnet test
```

---

## Wymogi

Mamy dane dwie encje (personalnie dla mnie strasznie confusing, dla mnie to "Entity", nie encja, troche zbaranialem jak to zobaczylem xd): **Order** i **OrderItem**.

```
+--------------+              +---------------+
|    ORDER     | 1          * |  ORDER ITEM   |
+--------------+--------------+---------------+
| net_total    +------------->| net_price     |
| tax          |              | quantity      |
| total        |              | net_total     |
+--------------+              | total         |
                              +---------------+
```

### Opis Encji

**Order:**
- `net_total` - wartość netto zamówienia (obliczane)
- `tax` - całkowita kwota podatku (obliczane)
- `total` - wartość brutto zamówienia (obliczane)

**OrderItem:**
- `net_price` - cena netto 1 sztuki towaru (dane wejściowe)
- `quantity` - ilość sztuk (dane wejściowe)
- `net_total` - wartość netto pozycji (obliczane)
- `total` - wartość brutto pozycji (obliczane)

### Wymagania Zadania

1. **Jakiego typu danych użyjesz do przechowywania poszczególnych wartości w bazie danych?**

2. **Napisz kod (+testy), który wypełni brakujące wartości dla obu encji mając dany Order z OrderItemami (podane `net_price` i `quantity`) oraz wysokość podatku w %.**

   **Podatek dla pojedynczego OrderItema powinien być liczony od wartości `net_total`.**

---

## Rozwiązanie

### Odpowiedź 1: Typy Danych

**`decimal`** dla wszystkich wartości pieniężnych (`net_price`, `net_total`, `tax`, `total`):
- Dokładna reprezentacja dziesiętna (brak błędów zmiennoprzecinkowych)
- Standard dla danych finansowych w .NET i bazach SQL
- Mapuje się na `DECIMAL(18,2)` lub `MONEY` w SQL Server, `NUMERIC` w PostgreSQL

**`int`** dla ilości (quantity):
- Standardowy typ całkowity dla policzalnych elementów
- Mapuje się na `INT` w bazach SQL

### Odpowiedź 2: Implementacja

✅ **Kod:** Klasa `OrderCalculator` z metodą `Calculate()`  
✅ **Testy:** 13 unit testów w xUnit  
✅ **Obliczanie podatku:** Podatek dla każdego OrderItem liczony od wartości `net_total`

**Formuła zgodnie z wymaganiami:**
```csharp
item.NetTotal = item.NetPrice × item.Quantity;
item.Total = Math.Round(item.NetTotal × (1 + TaxRate), 2);
// ← Podatek liczony od NetTotal ✅
```

---


## Model Danych

### Order
- `decimal NetTotal` - suma wartości netto wszystkich pozycji (obliczane)
- `decimal Tax` - całkowita kwota podatku (obliczane)
- `decimal Total` - suma brutto (obliczane)
- `List<OrderItem> Items` - kolekcja pozycji zamówienia

### OrderItem
- `decimal NetPrice` - cena jednostkowa przed opodatkowaniem (dane wejściowe)
- `int Quantity` - liczba jednostek (dane wejściowe)
- `decimal NetTotal` - cena netto × ilość (obliczane)
- `decimal Total` - wartość netto z podatkiem (obliczane)

## Kluczowe Decyzje Projektowe

### Logika Obliczania Podatku
```csharp
// Dla każdej pozycji:
NetTotal = NetPrice × Quantity
Total = Round(NetTotal × (1 + TaxRate), 2)

// Dla zamówienia:
Order.NetTotal = Sum(Items.NetTotal)
Order.Total = Sum(Items.Total)
Order.Tax = Order.Total - Order.NetTotal
```

- Podatek jest obliczany i zaokrąglany **per pozycja** (standardowa praktyka księgowa)
- Sumy zamówienia są agregowane z wartości pozycji
- Używa `MidpointRounding.AwayFromZero` dla zaokrągleń finansowych

**Framework:** .NET 8.0 
