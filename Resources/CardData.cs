using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public CardData() {
        data = new List<Card>() {
            Card.Creature("0001", "Card/Beholder", "Beholder", 270, Card.Role.rook, new Card.Attack[]{new Card.Attack("Laser Beam", 30, 2), new Card.Attack("Eye crash", 90, 5)}, "Monsters/Beholder/PrefabBeholder"),
            Card.Creature("0002", "Card/Bug", "Bug", 300, Card.Role.knight, new Card.Attack[]{new Card.Attack("Bite", 50, 3), new Card.Attack("Sting", 60, 4)}, "Monsters/Bug/PrefabBug"),
            Card.Creature("0003", "Card/ChestMonster", "Chest Monster", 250, Card.Role.rook, new Card.Attack[]{new Card.Attack("Bite", 50, 3), new Card.Attack("headbutt", 70, 4)}, "Monsters/ChestMonster/PrefabChestMonster"),
            Card.Creature("0004", "Card/Diatryma", "Diatryma", 390, Card.Role.bishop, new Card.Attack[]{new Card.Attack("Bite", 50, 3)}, "Monsters/Diatryma/PrefabDiatryma"),
            Card.Creature("0005", "Card/Dragonewt", "Dragonewt", 230, Card.Role.bishop, new Card.Attack[]{new Card.Attack("Arm Swipe", 120, 6), new Card.Attack("Bite", 50, 3) }, "Monsters/Dragonewt/PrefabDragonewt"),
            Card.Creature("0006", "Card/Fish", "Fish", 150, Card.Role.knight, new Card.Attack[]{new Card.Attack("Bite", 50, 3), new Card.Attack("Charging Bite", 250, 6), new Card.Attack("Tail Slap", 80, 4) }, "Monsters/Fish/PrefabFish"),

            Card.Authority("0007", "Card/authority", 1),
        };
    }

    public Card findCardById(string id) {
        foreach (Card a in data) {
            if (a.id == id) return a;
        }
        //kalo gak ketemu
        return data[0];
    }

    public List<Card> data;

    public class Card {

        public enum Type { creature, authority, effect }
        public enum Role { rook, knight, bishop }
        public class Attack
        {
            public Attack(string name, int damage, int requredAuthority) {
                this.name = name;
                this.damage = damage;
                this.requredAuthority = requredAuthority;
            }

            public string name;
            public int damage;
            public int requredAuthority;
        }

        //=================================================================
        public string id;
        public string imageAssets;
        public string name;
        public Type type;

        //if creature
        public int hp;
        public Attack[] attack;
        public Role role;
        public string monsterPrefabAssets;  //assets lokasi untuk init prefab saat monster dipanggil

        //if authority
        public int count; //besar authority
        //=================================================================

        /// <summary>
        /// Initiator creature
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageAssets"></param>
        /// <param name="name"></param>
        /// <param name="hp"></param>
        /// <param name="role"></param>
        /// <param name="attack"></param>
        /// <returns></returns>
        static public Card Creature(string id, string imageAssets, string name, int hp, Role role, Attack[] attack, string monsterPrefabAssets) {
            Card ret = new Card();
            ret.id = id;
            ret.imageAssets = imageAssets;
            ret.name = name;
            ret.hp = hp;
            ret.role = role;
            ret.attack = attack;
            ret.type = Type.creature;
            ret.monsterPrefabAssets = monsterPrefabAssets;

            return ret;
        }


        static public Card Authority(string id, string imageAssets, int count) {
            Card ret = new Card();
            ret.id = id;
            ret.imageAssets = imageAssets;
            ret.type = Type.authority;
            ret.count = count;

            return ret;
        }
    }
}
