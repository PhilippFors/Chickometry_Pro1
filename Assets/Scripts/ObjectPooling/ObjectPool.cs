using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UsefulCode.Utilities;

namespace ObjectPooling
{
    public class ObjectPool<T> : SingletonBehaviour<ObjectPool<T>> where T : Component
    {
        private readonly Queue<T> pool = new Queue<T>();

        [SerializeField, Range(1, 300)] private int initialSize = 3;
        [SerializeField] protected T prefab;
        [SerializeField] protected float pruneTime;
        [SerializeField] protected int pruneTolerance;
        private static int InstanceIdCounter;

        private float timer;
        private void Awake()
        {
            base.Awake();
            InitPool();
        }

        private void Update()
        {
            if (pool.Count > initialSize + pruneTolerance) {
                timer += Time.deltaTime;
                if (timer >= pruneTime) {
                    timer = 0;
                    var diff = pool.Count - initialSize;
                    for (int i = 0; i < diff; i++) {
                        var obj = pool.Dequeue();
                        Destroy(obj.gameObject);
                    }
                }
            }
        }

        private void InitPool()
        {
            for (int i = 0; i < initialSize; i++) {
                var newObj = InstantiateObject(false);
                newObj.transform.parent = transform;
                pool.Enqueue(newObj);
            }
        }

        public T GetObject(bool setActive = true)
        {
            T result = null;
            if (pool.Count == 0) {
                result = InstantiateObject(setActive);
            }
            else {
                var obj = pool.Dequeue();
                GameObject go = obj.gameObject;
                go.SetActive(setActive);
                result = obj;
            }

            return result;
        }

        /// <summary>
        /// Returns a used object to the pool.
        /// </summary>
        /// <param name="obj"></param>
        public void ReleaseObject(T obj)
        {
            obj.gameObject.SetActive(false);

            Assert.IsFalse(pool.Contains(obj), "Trying to release object multiple times.");

            pool.Enqueue(obj);
            obj.transform.parent = transform;
        }

        protected virtual T InstantiateObject(bool setActive)
        {
            var newObj = Instantiate(prefab, transform);
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(setActive);
            newObj.name = $"{newObj.name} [{InstanceIdCounter++}]";
            return newObj;
        }
    }
}