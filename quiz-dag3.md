# Quiz: EF Core over dag 2

1. Gegeven een class-hiërarchie met een basisclass Person en subklassen VerzekeringsAgent en Klant. Alle personen moeten worden opgehaald, inclusief hun specifieke eigenschappen (zoals Polissen voor Klant en Kantoor voor VerzekeringsAgent).

   Welke van de onderstaande codevoorbeelden haalt alle personen met hun subclass-data correct op?

   A)

   ```cs
   var personen = context.Persons.ToList();
   ```

   B)

   ```cs
   var personen = context.Persons.Include("Polissen").Include("Kantoor").ToList();
   ```

   C)

   ```cs
   var personen = context.Persons.OfType<Klant>().Include(p => p.Polissen).ToList();
   ```

   D)

   ```cs
   var personen = context.Persons.Include(p => p.Polissen).ToList();
   ```

1. Wat is de standaard `DeleteBehavior` voor een relatie met een optionele foreign key?  
   A) Cascade  
   B) Restrict  
   C) ClientSetNull  
   D) NoAction

1. Wat moet worden toegevoegd om deze query te kunnen uitvoeren?

   ```cs
   var results = context.People
      .Where(p => p.Naam.StartsWith("A") && CustomFilter(p))
      .ToList();
   ```

1. Max werkt met een systeem voor verzekeringen. Elke polis wordt uniek geïdentificeerd door een combinatie van PolisNummer en KlantId (composite key).

   Max wil één specifieke polis ophalen uit de database met Entity Framework. Hij gebruikt:

   A.

   ```cs
   var polis = context.Polissen.Find(polisNummer);
   ```

   B.

   ```cs
   var polis = context.Polissen.Find(polisNummer, klantId);
   ```

   C.

   ```cs
   var polis = context.Polissen.Where(p => p.PolisNummer == polisNummer && p.KlantId == klantId);
   ```

   D.

   ```cs
   var polis = context.Polissen.Find(new { PolisNummer = polisNummer, KlantId = klantId });
   ```

1. Een lijst van polissen moet worden opgehaald voor een rapport.De gegevens worden alleen gelezen en niet aangepast. De performance moet worden geoptimaliseerd. Welke code is het meest geschikt?

   A.

   ```cs
   var polissen = context.Polissen.ToList();
   ```

   B.

   ```cs
   var polissen = context.Polissen.AsNoTracking().ToList();
   ```

   C.

   ```cs
   context.ChangeTracker.Clear();
   var polissen = context.Polissen.ToList();
   ```

   D.

   ```cs
   var polissen = context.Polissen.Attach().ToList();
   ```

1. Wat gebeurt er precies als je `AddRange` gebruikt in EF Core?

   A) Alle entiteiten worden toegevoegd aan de Change Tracker met de status Added  
   B) Alleen de eerste entiteit wordt toegevoegd aan de Change Tracker  
   C) Alle entiteiten worden direct naar de database geschreven, zonder Change Tracker  
   D) Alleen bestaande entiteiten worden bijgewerkt

1. Welke `DeleteBehaviour` zorgt ervoor dat gerelateerde entiteiten worden verwijderd als de hoofdentiteit wordt verwijderd?

   A) `Restrict`  
   B) `SetNull`  
   C) `Cascade`  
   D) `NoAction`

1. Welke van de onderstaande acties kun je NIET direct uitvoeren met een `DbContext` in EF Core?

   A) Entiteiten toevoegen, wijzigen en verwijderen  
   B) Databaseverbindingen beheren  
   C) Direct een database-back-up maken  
   D) Queries uitvoeren op entiteiten

1. Fill in the gap. Maak een nieuwe student aan en voeg deze toe aan de context:

   ```csharp
   var student = new Student { Name = "Piet" };
   context.__________(student);
   context.___________();
   ```

1. Waarom werkt deze code wel/niet?

   ```csharp
   var count = context.Students.CountAsync();
   ```

1. Er bestaat een many-to-many relatie tussen Student en Course zonder een expliciete join class. Welke aanpak is voldoende om deze relatie in EF Core te configureren?

   A) Navigatieproperties in beide classes; EF Core maakt automatisch een join table aan.  
   B) Navigatieproperties in beide classes én een expliciete join class toevoegen.  
   C) Navigatieproperties in beide classes, maar handmatig een join table in de database aanmaken.  
   D) Alleen een join class toevoegen zonder navigatieproperties in Student en Course.

1. Gegeven de volgende entiteiten:

   ```cs
   public class Klant
   {
       public int Id { get; set; }
       public string Naam { get; set; }
       public List<Polis> Polissen { get; } = [];
   }

   public class Polis
   {
       public int Id { get; set; }
       public string Type { get; set; }
   }
   ```

   Hoe wordt deze relatie genoemd?

   A) Self-referencing
   B) Unidirectional
   C) Bidirectional
   D) Composition

   Je wilt een lijst ophalen van klanten met hun naam en het aantal polissen dat ze hebben.

   A)

   ```cs
   var result = context.Klanten
   .Select(k => new
   {
   k.Naam,
   AantalPolissen = k.Polissen.Count()
   });
   ```

   B)

   ```cs
   var result = context.Klanten
   .Select(k => k.Polissen.Count());
   ```

   C)

   ```cs
   var result = context.Klanten
   .Count(k => k.Polissen)
   .ToList();
   ```

   D)

   ```cs
   var result = context.Klanten
   .Select(k => new
   {
   Naam = k,
   AantalPolissen = Count(k.Polissen)
   })
   .ToList();
   ```

```

```
