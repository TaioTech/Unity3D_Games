/// <summary>
/// Attribute.cs
/// Nov 28, 2017
/// Tyler Carolan
/// 
/// This is the class for all the character attributes in game
/// </summary>
public class Attribute : BaseStat {


	private string _name;

	public Attribute() {
		_name = "";
		ExpToLevel = 50;
		LevelModifier = 1.05f;
	}

	public string Name {
		get { return _name; }
		set { _name = value; }
	}
}


public enum AttributeName {
	Might,
	Constitution,
	Nimbleness,
	Speed,
	Concentration,
	Willpower,
	Charisma
}