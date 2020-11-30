using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Video;
using Object = UnityEngine.Object;
using flash.display;

namespace Utils {
    public class ResUtils {
        //缓存 prefab
        private static Dictionary<string, GameObject> _prefabGameObjectCache = new Dictionary<string, GameObject> ();
        private static Dictionary<string, UnityEngine.Sprite> _spriteCache = new Dictionary<string, UnityEngine.Sprite> ();
        //private static Dictionary<string, UnityEngine.U2D.SpriteAtlas> _spriteAltasCache = new Dictionary<string, UnityEngine.U2D.SpriteAtlas> ();
        private static Dictionary<string, UnityEngine.TextAsset> _textFileCache = new Dictionary<string, UnityEngine.TextAsset> ();

        public static void doSample () {
            //资源一定要放在 Resources 文件夹下
            GameObject _prefabObject = duplicatePrefab ("Prefabs/Lines/LineRendererObject");

            //加载精灵
            UnityEngine.Sprite sprite = Resources.Load<UnityEngine.Sprite> ("SkyTileSprite");
            GameObject go = new GameObject ("sp");
            go.AddComponent<SpriteRenderer> ().sprite = sprite;

            //加载Texture
            Texture tex = Resources.Load<Texture> ("SkyTileSprite");
            GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
            cube.GetComponent<MeshRenderer> ().material.mainTexture = tex;

            //加载Texture2D -> sprite
            Texture2D tex2d = Resources.Load<Texture2D> ("SkyTileSprite");
            UnityEngine.Sprite sp = UnityEngine.Sprite.Create (tex2d, new Rect (0, 0, tex2d.width, tex2d.height), new Vector2 (0.5f, 0.5f));

            //加载声音类型
            AudioClip clip = Resources.Load<AudioClip> ("mp3");
            //加载视频类型
            VideoPlayer movie = Resources.Load<VideoPlayer> ("movie");

            //Unity的GC
            Resources.UnloadAsset (tex);
            //释放unity认为不使用的资源
            Resources.UnloadUnusedAssets ();
        }
        //复制 prefab <资源一定要放在 Assets/Resources/ 文件夹下>
        public static GameObject duplicatePrefab (string prefabPath_) {
            GameObject _pfb = null;
            if (_prefabGameObjectCache.ContainsKey (prefabPath_)) { //有缓存就用缓存的
                _pfb = _prefabGameObjectCache[prefabPath_];
            } else { //没有缓存，就创建一个，缓存下来
                _pfb = Resources.Load<GameObject> (prefabPath_);
                _prefabGameObjectCache.Add (prefabPath_, _pfb);
            }
            //没有创建出来就报错
            if (_pfb == null) {
                Debug.LogError ("ResUtils -> duplicatePrefab " + prefabPath_ + " 失败");
                return null;
            }
            //通过缓存对象复制对象
            GameObject _createObject = Object.Instantiate (_pfb);
            //创建出来就返回
            return _createObject;
        }

        //将 resources 文件下的某个文件夹加载成 TextAssets 对象并缓存
        public static void cacheTextInFolder (string folderPathInResources_) {
            Object[] _textFiles = Resources.LoadAll (folderPathInResources_, typeof (UnityEngine.TextAsset));
            foreach (var _textFile in _textFiles){
                _textFileCache[_textFile.name] = _textFile as UnityEngine.TextAsset;
            }
        }
        public static void cacheSpriteInFolder (string folderPathInResources_) {
            preloadFolderAsSprite (folderPathInResources_); //先读取一下普通的。
            preloadFolderAsSpriteAtlas (folderPathInResources_); //在读取一下大图。
        }

        //将 resources 文件下的某个文件夹加载成 Sprite 对象并缓存
        public static void preloadFolderAsSprite (string folderPathInResources_) {
            Object[] _sprites = Resources.LoadAll (folderPathInResources_, typeof (UnityEngine.Sprite));
            foreach (var _sp in _sprites) {
                _spriteCache[_sp.name+"(Clone)"] = _sp as UnityEngine.Sprite;
            }
        }
        //将 resources 文件下的某个文件夹加载成 SpriteAtlas 对象并缓存
        public static void preloadFolderAsSpriteAtlas (string folderPathInResources_) {
            Object[] _spriteAltas = Resources.LoadAll (folderPathInResources_, typeof (UnityEngine.U2D.SpriteAtlas));
            foreach (var _spAltas in _spriteAltas) {
                //_spriteAltasCache[_spAltas.name] = _spAltas as UnityEngine.U2D.SpriteAtlas;
                UnityEngine.Sprite[] _spriteArray = new UnityEngine.Sprite[(_spAltas as UnityEngine.U2D.SpriteAtlas).spriteCount];
                (_spAltas as UnityEngine.U2D.SpriteAtlas).GetSprites (_spriteArray);
                foreach (var _sp in _spriteArray) { //替换掉原有的
                    _spriteCache[_sp.name] = _sp;
                }
            }
        }

        //从缓存中获取Sprite
        public static UnityEngine.Sprite getSpriteByName (string spriteName_) {
            UnityEngine.Sprite _sprite;
            if (!_spriteCache.TryGetValue (spriteName_+"(Clone)", out _sprite)) {
                Debug.LogError ("ERROR " + System.Reflection.MethodBase.GetCurrentMethod ().ReflectedType.FullName + " -> " + new System.Diagnostics.StackTrace ().GetFrame (0).GetMethod ().Name + " : " +
                    spriteName_ + " 在ResUtils中并没有相应的缓存"
                );
                return null;
            }
            return _sprite;
        }
    }
}