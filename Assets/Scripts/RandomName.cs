using UnityEngine;

public static class RandomName
{
    public class NamesList
    {
        public string[] nicknames = null;
    }

    private static TextAsset names;

    private static NamesList namesList;

    public static string Name
    {
        get
        {
            var array = GetRandomName().ToCharArray();
            array[0] = array[0].ToString().ToUpper().ToCharArray()[0];
            return new string(array);
        }
    }

    private static string GetRandomName()
    {
        if (names == null)
            names = Resources.Load<TextAsset>("nicknames");
        if (namesList == null)
            namesList = JsonUtility.FromJson<NamesList>(names.text);

        return namesList.nicknames[Random.Range(0, namesList.nicknames.Length)];
    }
}