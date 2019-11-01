
public class Animal 
{
    string animalname;
    string animalsize;


    public Animal() {
    }

    public Animal(string animalname,string animalsize)
    {
        this.animalname = animalname;
        this.animalsize = animalsize;
    }

    public string getmodelName()
    {
        return animalname;

    }

    public string getmodelsize()
    {
        return animalsize;

    }

    public void setanimalName(string name)
    {
        this.animalname = name;

    }
    public void setanimalSize(string size)
    {
        this.animalsize = size;
    }

}
