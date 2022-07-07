using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Firebase.Firestore;

/// <summary>
/// Kelas ini berisi fungsi-fungsi yang dipakai lebih dari 1 kelas
/// </summary>
public class repo
{

    public class MyConstant {
        public const int monsterScale = 18;     //ukuran monster, variable ini belum terpakai di program
        public const int gridSize = monsterScale + (monsterScale / 2);
    }


    public static FirebaseFirestore _db;
    public static FirebaseFirestore db
    {
        get
        {
            if (_db == null) _db = FirebaseFirestore.DefaultInstance;
            return _db;
        }
    }



    /// <summary>
    /// Fungsi ini mengecek apa yang sedang ditunjuk objek cursor pada panel
    /// </summary>
    /// <param name="cursor"></param>
    /// <returns>null jika tidak menunjuk collider apapun</returns>
    public static Collider raycastToPanel(GameObject cursor)
    {
        RaycastHit hit;
        Ray landingRay = new Ray(cursor.transform.position, Vector3.forward);
        if (Physics.Raycast(landingRay, out hit))
        {
            return hit.collider;
        }
        return null;
    }


    /// <summary>
    /// menghapus semua child dari suatu parrent
    /// </summary>
    /// <param name="parent">Gameobject yang mau dikosongkan</param>
    public static void destroyAllChild(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// kelas berisi fungsi-fungsi untuk mengetahui apakah suatu aksi dapat dilakukan
    /// </summary>
    public class Validator {

        public class attackTarget {
            public bool canAttack {
                get {
                    if (targetsPosition.Count == 0) return false;
                    return true;
                }
            }
            public List<Vector2Int> targetsPosition = new List<Vector2Int>();
        }

        public static bool isMoveValid(CardData.Card.Role role, Vector2Int origin, Vector2Int destination)
        {
            if (role == CardData.Card.Role.rook)
            {
                if (origin.x == destination.x || origin.y == destination.y) return true;
            }
            else if (role == CardData.Card.Role.bishop)
            {
                //artinya nyari SELISIH = Math.Abs(origin.x - destination.x)
                if (Math.Abs(origin.x - destination.x) == Math.Abs(origin.y - destination.y)) return true;
            }
            else if (role == CardData.Card.Role.knight) {
                if (origin.x + 2 == destination.x || origin.x - 2 == destination.x)
                {
                    if (origin.y + 1 == destination.y || origin.y - 1 == destination.y) return true;
                }
                else if (origin.x + 1 == destination.x || origin.x - 1 == destination.x) {
                    if (origin.y + 2 == destination.y || origin.y - 2 == destination.y) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// untuk mengecek apakah bisa menyerang atau tidak, sekaligus memberikan posisi monster yang bisa diserang
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="origin">asal pion(digunakan untuk knight karena jika tidak ada ini, knight akan mendeteksi dirinya sendiri)</param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static attackTarget canAttack(Vector2Int destination, Vector2Int origin, CardData.Card.Role role) {

            var ret = new attackTarget();
            int[] bilangan1;
            int[] bilangan2;

            if (role == CardData.Card.Role.rook)
            {
                bilangan1 = new int[] { 1, -1, 0, 0 };
                bilangan2 = new int[] { 0, 0, 1, -1 };
            }
            else if (role == CardData.Card.Role.bishop)
            {
                bilangan1 = new int[] { 1, -1, 1, -1 };
                bilangan2 = new int[] { 1, 1, -1, -1 };
            }
            else
            {
                bilangan1 = new int[] { -1, -1, 1, 1, -2, -2, 2, 2 };
                bilangan2 = new int[] { -2, 2, -2, 2, -1, 1, -1, 1 };
            }

            for (int x = 0; x < bilangan1.Length; x++)
            {
                try
                {
                    //skip kalo lokasi pion  yang ditemukan adalah lokasi awal(lokasi sendiri)
                    if (destination.x + bilangan1[x] == origin.y && destination.y + bilangan2[x] == origin.x)
                    {
                        continue;
                    }

                    var data = duelGamePlay.CardInP2[destination.x + bilangan1[x], destination.y + bilangan2[x]];
                    if (data.isOccupied)
                    {
                        //skip kalo pion yang dapat diserang milik sendiri(gak boleh serang teman)
                        if (duelGamePlay.cardInP3[data.panel3Index].isBlue == DuelFirestore.meIsBlue)
                        {
                            continue;
                        }
                        ret.targetsPosition.Add(new Vector2Int(destination.x + bilangan1[x], destination.y + bilangan2[x]));
                    }
                }
                catch { }
            }

            return ret;
        }
    }
}
