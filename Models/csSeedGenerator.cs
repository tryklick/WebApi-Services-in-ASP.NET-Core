using System;
using System.Collections.Generic;
using System.Linq;
using static Models.GoodComment;

namespace Models
{
    public class GoodComment
    {
        public string Text { get; set; }

        public GoodComment() { }

        public GoodComment(string _text)
        {
            Text = _text;
        }

    }

    public interface ISeed<T>
    {
        //In order to separate from real and seeded instances
        public bool Seeded { get; set; }

        //Seeded The instance
        public T Seed(csSeedGenerator seedGenerator);
    }

    public class csSeedGenerator : Random
    {
        private string[] _firstnames = "John, Jane, Mike, Emily, David".Split(", ");
        private string[] _lastnames = "Doe, Smith, Johnson, Brown, Wilson".Split(", ");


        string[] _sightnames = "Gamla Stan, Vasa Museum, Vigeland Park, Holmenkollen, Helsinki Cathedral, Suomenlinna, Tivoli Gardens, Nyhavn, Blue Lagoon, Golden Circle".Split(", ");

        private string[] _sightdescription =
         {
            "Historic city center",
            "Museum with a 17th-century ship",
            "Sculpture park",
            "Ski jump and museum",
            "Iconic Lutheran cathedral",
            "Sea fortress",
            "Amusement park",
            "Colorful waterfront district",
            "Geothermal spa",
            "Tourist route with natural wonders"
        };

        string[][] _address =
         {
                "Drottninggatan, Storgatan, Linnégatan, Norra Esplanaden, Karl Johans gate , Strandgata , Skippergata".Split(", "),
                ", Mannerheimintie , Aleksanterinkatu,Kauppakatu, Rauhankatu, Strøget, Østergade ,H. C. Andersens Boulevard,Algade 32".Split(", "),
                "Laugavegur, Bankastræti, Austurvegur, Kirkjubraut, Sveavägen, Kungsgatan , Köpmangatan".Split(", "),
                "Ågatan, Grensen, Sentrumsveien, Kirkegata, Hovioikeudenpuistikko, Rauhankatu,Austurvegur".Split(", ")
         };


        string[][] _city = {"Stockholm,Oslo, Helsinki, Copenhagen, Reykjavik".Split(", ") };

        string[] _country = "Sweden, Norway,Finland,Denmark,Iceland".Split(", ");

        string[] _domains = "icloud.com, me.com, mac.com, hotmail.com, gmail.com".Split(", ");

       GoodComment[] _text =
       {
            new GoodComment("Loved the charming alleys"),
            new GoodComment("A true gem in the city"),
            new GoodComment("The Vasa ship is amazing"),
            new GoodComment("I learned a lot about Swedish history"),
            new GoodComment("The sculptures are so creative"),
            new GoodComment("I could spend hours here"),
            new GoodComment("What a thrilling experience"),
            new GoodComment("The museum is very informative"),
            new GoodComment("The view is breathtaking"),
            new GoodComment("I was scared at first, but it was so worth it"),
            new GoodComment("The alleys are so picturesque"),
            new GoodComment("Charming atmosphere in the city"),
            new GoodComment("Great local shops to explore"),
            new GoodComment("Impressive historical ship"),
            new GoodComment("A must-visit for history enthusiasts"),
            new GoodComment("The Vasa ship tells a fascinating story"),
            new GoodComment("Artistic sculptures all around"),
            new GoodComment("Perfect spot for photography"),
            new GoodComment("Unique and creative artworks"),
            new GoodComment("Thrilling adventure at great heights"),
            new GoodComment("Breathtaking views from the top"),
            new GoodComment("An adrenaline rush like no other"),
            new GoodComment("Unforgettable panoramic view"),
            new GoodComment("Worth the hike for the stunning scenery"),
            new GoodComment("Nature at its finest")
       };

        public string SightName => _sightnames[this.Next(0, _sightnames.Length)];
        public string SightDescription => _sightdescription[this.Next(0, _sightdescription.Length)];


        public string FirstName => _firstnames[this.Next(0, _firstnames.Length)];
        public string LastName => _lastnames[this.Next(0, _lastnames.Length)];
        public string FullName => $"{FirstName} {LastName}";

        public bool Bool => (this.Next(0, 101) < 51) ? true : false;

        public string Email(string fname = null, string lname = null)
        {
            fname ??= FirstName;
            lname ??= LastName;

            return $"{fname}.{lname}@{_domains[this.Next(0, _domains.Length)]}";
        }

        public string Country => _country[this.Next(0, _country.Length)];
        

        public string City(string Country = null)
        {

            var cIdx = this.Next(0, _city.Length);
            if (Country != null)
            {
                //Give a City in that specific country
                cIdx = Array.FindIndex(_country, c => c.ToLower() == Country.Trim().ToLower());

                if (cIdx == -1) throw new Exception("Country not found");
            }

            return _city[cIdx][this.Next(0, _city[cIdx].Length)];
        }

        public string StreetAddress(string Country = null)
        {

            var cIdx = this.Next(0, _city.Length);
            if (Country != null)
            {
                //Give a City in that specific country
                cIdx = Array.FindIndex(_country, c => c.ToLower() == Country.Trim().ToLower());

                if (cIdx == -1) throw new Exception("Country not found");
            }

            return $"{_address[cIdx][this.Next(0, _address[cIdx].Length)]} {this.Next(1, 51)}";
        }

        public int ZipCode => this.Next(10101, 100000);

        #region Seed from own datastructures
        public TEnum FromEnum<TEnum>() where TEnum : struct
        {
            if (typeof(TEnum).IsEnum)
            {

                var _names = typeof(TEnum).GetEnumNames();
                var _name = _names[this.Next(0, _names.Length)];

                return Enum.Parse<TEnum>(_name);
            }
            throw new ArgumentException("Not an enum type");
        }
        public TItem FromList<TItem>(List<TItem> items)
        {
            return items[this.Next(0, items.Count)];
        }

        internal string FromList(string[] names)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region generate seeded Lists
        public List<TItem> ToList<TItem>(int NrOfItems)
            where TItem : ISeed<TItem>, new()
        {
            //Create a list of seeded items
            var _list = new List<TItem>();
            for (int c = 0; c < NrOfItems; c++)
            {
                _list.Add(new TItem().Seed(this));
            }
            return _list;
        }

        public List<TItem> ToListUnique<TItem>(int tryNrOfItems, List<TItem> appendToUnique = null)
             where TItem : ISeed<TItem>, IEquatable<TItem>, new()
        {
            //Create a list of uniquely seeded items
            HashSet<TItem> _set = (appendToUnique == null) ? new HashSet<TItem>() : new HashSet<TItem>(appendToUnique);

            while (_set.Count < tryNrOfItems)
            {
                var _item = new TItem().Seed(this);

                int _preCount = _set.Count();
                int tries = 0;
                do
                {
                    _set.Add(_item);
                    if (++tries >= 5)
                    {
                        //it takes more than 5 tries to generate a random item.
                        //Assume this is it. return the list
                        return _set.ToList();
                    }
                } while (!(_set.Count > _preCount));
            }

            return _set.ToList();
        }


        public List<TItem> FromListUnique<TItem>(int tryNrOfItems, List<TItem> list = null)
        where TItem : ISeed<TItem>, IEquatable<TItem>, new()
        {
            //Create a list of uniquely seeded items
            HashSet<TItem> _set = new HashSet<TItem>();

            while (_set.Count < tryNrOfItems)
            {
                var _item = list[this.Next(0, list.Count)];

                int _preCount = _set.Count();
                int tries = 0;
                do
                {
                    _set.Add(_item);
                    if (++tries >= 5)
                    {
                        //it takes more than 5 tries to generate a random item.
                        //Assume this is it. return the list
                        return _set.ToList();
                    }
                } while (!(_set.Count > _preCount));
            }

            return _set.ToList();
        }

        #endregion
        #region Comment
        public List<GoodComment> Allcomments => _text.ToList();
        public GoodComment Comment => _text[this.Next(0, _text.Length)];
        #endregion

    }
}

