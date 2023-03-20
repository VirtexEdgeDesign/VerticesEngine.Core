using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Entities
{
    public class vxObjectPool
    {
        public static vxObjectPool Instance
        {
            get { return m_instance; }
        }
        private static vxObjectPool m_instance = new vxObjectPool();

        internal Dictionary<Type, vxObjectSubPool> m_objectPool = new Dictionary<Type, vxObjectSubPool>();


        private vxObjectPool()
        {

        }

        /// <summary>
        /// Initialises the Object bool for the given 
        /// </summary>
        internal void InitScene()
        {
            foreach(var pool in vxEntityRegister.ObjectPools)
            {
                if(!m_objectPool.ContainsKey(pool.Key))
                m_objectPool.Add(pool.Key, new vxObjectSubPool(pool.Value));
            }
            //for (int i = 0; i < poolSize; i++)
            //{
            //    vxIParticle particle;

            //    System.Reflection.ConstructorInfo ctor = typeof(T).GetConstructor(new[] { typeof(vxGameplayScene3D) });

            //    // if there isn't this constructor, then there should be one with just the scene
            //    if (ctor == null)
            //    {
            //        throw new Exception("Missing contructor for " + typeof(T).ToString());
            //    }
            //    else
            //    {
            //        particle = (vxIParticle)ctor.Invoke(new object[] { vxEngine.Instance.CurrentScene });
            //        newPool.Pool.Add(particle);
            //    }
            //}

        }

        internal void OnSceneEnd()
        {
            m_objectPool.Clear();
        }


        /// <summary>
        /// Spawn a new object from the internal pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Spawn<T>() where T : vxEntity
        {
            var lookUpType = typeof(T);

            if(m_objectPool.TryGetValue(lookUpType, out var pool))
            {
                return (T)pool.SpawnNext();
            }

            return null;
        }


        /// <summary>
        /// Recycles an item in the object pool, deactivating it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Recycle<T>(T entity) where T : vxEntity
        {
            var lookUpType = typeof(T);

            if (m_objectPool.TryGetValue(lookUpType, out var pool))
            {
                pool.Recycle(entity);
            }
        }
    }


    internal class vxObjectPoolDefinition
    {
        public readonly int Count;
        public readonly Type type;

        public vxObjectPoolDefinition(Type type, int count)
        {
            this.type = type;
            Count = count;
        }
    }

    internal class vxObjectSubPool
    {
        public int MaxCount
        {
            get { return m_entities.Count; }
        }

        private List<vxEntity> m_entities = new List<vxEntity>();

        public int m_currentObjectIndex = 0;

        public vxObjectSubPool(vxObjectPoolDefinition definition)
        {
            m_entities = new List<vxEntity>(definition.Count);

            for (int i = 0; i < definition.Count; i++)
            {
                vxEntity obj;

                var tp = definition.type;

                System.Reflection.ConstructorInfo ctor = tp.GetConstructor(Type.EmptyTypes);

                // if there isn't this constructor, then there should be one with just the scene
                if (ctor == null)
                {
                    throw new Exception("Missing contructor for " + tp.ToString());
                }
                else
                {
                    obj = (vxEntity)ctor.Invoke(new object[] { });
                    obj.IsEnabled = false;
                    m_entities.Add(obj);
                }
            }
        }



        public vxEntity SpawnNext()
        {
            int index = m_currentObjectIndex % m_entities.Count;
            var entity = m_entities[index];
            m_currentObjectIndex++;
            // enable the entity
            entity.IsEnabled = true;

            return entity;
        }

        public void Recycle(vxEntity entity)
        {
            entity.IsEnabled = false;
        }
    }
}
