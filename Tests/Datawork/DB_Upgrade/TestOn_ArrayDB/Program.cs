using System.Linq;
using Monsajem_Incs.Database;
using Monsajem_Incs.Database.Base;
using System;
using System.Threading.Tasks;
using Monsajem_Incs.Collection.Array.ArrayBased.Hyper;

namespace TestOn_ArrayDB
{
    public class OldDataBase
    {
        public OldDataBase()
        {
            Persons = new ArrayTable<Person, string>(
                (person) => person.Name, false);
            (Persons.Relation((c) => c.Frinds), Persons.Relation((c) => c.Frinds)).Join();
            (Persons.Relation((c) => c.Dad), Persons.Relation((c) => c.Child)).Join();
        }

        public Table<Person, string> Persons;

        public class Person
        {
            public string Name;
            public PartOfTable<Person, string> Frinds;
            public Table<Person, string>.RelationItem Dad;
            public Table<Person, string>.RelationItem Child;
        }
    }

    public class NewDataBase
    {
        public NewDataBase()
        {
            Persons = new ArrayTable<Person, string>(
                (person) => person.Name, false);
            (Persons.Relation((c) => c.Frinds), Persons.Relation((c) => c.Frinds)).Join();
            (Persons.Relation((c) => c.Dad), Persons.Relation((c) => c.Child)).Join();
        }

        public Table<Person, string> Persons;

        public class Person
        {
            public string Name;
            public PartOfTable<Person, string> Frinds;
            public Table<Person, string>.RelationItem Dad;
            public Table<Person, string>.RelationItem Child;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var OldDB = new OldDataBase();


            OldDB.Persons.Insert((c) => c.Name = "dave");
            OldDB.Persons.Insert((c) => c.Name = "john");
            OldDB.Persons.Insert((c) => c.Name = "razor");
            OldDB.Persons.Insert((c) => c.Name = "karen");
            OldDB.Persons.Insert((c) => c.Name = "joe");

            OldDB.Persons[0].Value.Frinds.Accept(OldDB.Persons[1]);
            OldDB.Persons.Update(0,(c)=> { c.Dad = OldDB.Persons[3].Value; });



            var NewDB = new NewDataBase();

            DatabaseUpgrider.UpgridDatabase(OldDB,NewDB);
        }

    }
}
