using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName ="Managers/Resources Manager")]
    public class ResourcesManager : ScriptableObject
    {
        public List<Item> allItems = new List<Item>();
        Dictionary<string, Item> itemDict = new Dictionary<string, Item>();

        public RoomVariable currentRoom;
        public MapVariable currentMap;

        public void Init()
        {
            for(int i= 0; i < allItems.Count; i++)
            {
                if (!itemDict.ContainsKey(allItems[i].name))
                {
                    itemDict.Add(allItems[i].name, allItems[i]);
                }
                else
                {
                    Debug.Log("There are two item name : " + allItems[i].name);
                }
            }
        }

        public Item GetItemInstance(string targetID)
        {
            Item defaultItem = GetItem(targetID);
            Item newItem = Instantiate(defaultItem);
            newItem.name = defaultItem.name;
            return newItem;
        }

        public ClothItem GetClothItem(string targetId)
        {
            ClothItem clothItem = (ClothItem)GetItem(targetId);
            return clothItem;
        }

        public List<ClothItem> GetAllCloth()
        {
            List<ClothItem> r = new List<ClothItem>();
            foreach(Item i in allItems)
            {
                if(i is ClothItem)
                {
                    r.Add((ClothItem)i);
                }
            }
            return r;
        }

        public List<Weapon> GetAllWeapon()
        {
            List<Weapon> r = new List<Weapon>();
            foreach (Item i in allItems)
            {
                if (i is Weapon)
                {
                    r.Add((Weapon)i);
                }
            }
            return r;
        }

        Item GetItem(string targetID)
        {
            Item retVal = null;
            itemDict.TryGetValue(targetID, out retVal);
            return retVal;
        }

    }
}
