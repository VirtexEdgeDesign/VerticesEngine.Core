using System;
using System.Collections.Generic;
using System.Reflection;
using VerticesEngine.Entities;
using VerticesEngine.Particles;
using VerticesEngine.Plugins;
using VerticesEngine.Utilities;

namespace VerticesEngine
{
    /// <summary>
    /// Handles Entities and Particle registration from content packs
    /// </summary>
    public static class vxEntityRegister
    {
        /// <summary>
        /// The entity texture data. This is used by both 2D entities as well as particles and extra entities.
        /// </summary>
        public static Dictionary<object, vxEntitySpriteSheetDefinition> EntitySpriteSheetRegister = new Dictionary<object, vxEntitySpriteSheetDefinition>();


        /// <summary>
        /// The entity definitions.
        /// </summary>
        public static Dictionary<object, vxSandboxEntityRegistrationInfo> EntityDefinitions = new Dictionary<object, vxSandboxEntityRegistrationInfo>();


        /// <summary>
        /// The Particle pool definitions. This holds Particle types and pool sizes.
        /// </summary>
        public static Dictionary<string, vxParticlePoolDefinition> ParticleDefinitions = new Dictionary<string, vxParticlePoolDefinition>();


        public static Dictionary<string, vxSandboxEntityCategory> Categories = new Dictionary<string, vxSandboxEntityCategory>();


        public static vxSandboxEntitySubCategory GetSubCategory(object category, object subCat)
        {
            return Categories[category.ToString()].SubCategories[subCat];
        }


        public static vxSandboxEntitySubCategory GetSubCategory(vxRegisterAsSandboxEntityAttribute attribute)
        {
            return Categories[attribute.Category.ToString()].SubCategories[attribute.SubCategory.ToString()];
        }

        internal static Dictionary<Type, vxObjectPoolDefinition> ObjectPools = new Dictionary<Type, vxObjectPoolDefinition>();

        public static void RegisterAssemblyParticleSystems(Assembly assembly)
        {
            IEnumerable<Type> particleTypes = assembly.GetTypesWithAttribute(typeof(vxRegisterAsParticleSystemAttribute));


            // Organise Sandbox Items into Categories and Sub Category Lists
            foreach (var type in particleTypes)
            {
                var attr = type.GetCustomAttribute<vxRegisterAsParticleSystemAttribute>();

                var key = type.Name;

                var entityDef = new vxParticlePoolDefinition(type, attr.Name, key.ToString(), attr.PoolSize);

                // Particle Definition
                if (vxEntityRegister.ParticleDefinitions.ContainsKey(key.ToString()))
                {
                    vxConsole.WriteError(string.Format("ERROR LOADING PARTICLES: Particle '{0}' is using key '{1}' which already exists in the pool.", type.Name, key.ToString()));
                }
                else
                {
                    vxEntityRegister.ParticleDefinitions.Add(key.ToString(), entityDef);
                }
            }
        }

        /// <summary>
        /// Registers all Entities and Particles tagged with the appropriate 
        /// </summary>
        /// <param name="assembly"></param>
        internal static void RegisterAssemblyEntityTypes(vxIPlugin plugin)
        {
            Assembly assembly = Assembly.GetAssembly(plugin.GetType());

            //IEnumerable<Type> types = Assembly.GetAssembly(this.GetType()).GetTypesWithAttribute(typeof(vxRegisterAsSandboxEntityAttribute));
            IEnumerable<Type> types = assembly.GetTypesWithAttribute(typeof(vxRegisterAsSandboxEntityAttribute));


            // Organise Sandbox Items into Categories and Sub Category Lists
            foreach (var type in types)
            {
                vxRegisterAsSandboxEntityAttribute itemAttribute = type.GetCustomAttribute<vxRegisterAsSandboxEntityAttribute>();

                // first check if this category exists.
                if (vxEntityRegister.Categories.ContainsKey(itemAttribute.Category.ToString()) == false)
                    vxEntityRegister.Categories.Add(itemAttribute.Category.ToString(), new vxSandboxEntityCategory(itemAttribute.Category));

                // Now check if the Subcategory exists in the Category
                if (vxEntityRegister.Categories[itemAttribute.Category.ToString()].SubCategories.ContainsKey(itemAttribute.SubCategory.ToString()) == false)
                    vxEntityRegister.Categories[itemAttribute.Category.ToString()].SubCategories.Add(itemAttribute.SubCategory.ToString(), new vxSandboxEntitySubCategory(itemAttribute.SubCategory));


                var entityDef = new vxSandboxEntityRegistrationInfo(type, itemAttribute, ref plugin);// new vxEntityDefinition(type, name, categoryKey.ToString(), spritesheetLoc, EntityType);

                // now set the content pack key for all registered items
                //entityDef.SetContentPackInfo(new vxPluginMetaInfo(plugin));

                // Now finally add the type to the Sub Categories List of Types to own
                vxEntityRegister.GetSubCategory(itemAttribute).types.Add(entityDef);


                // Item Definition
                vxEntityRegister.EntityDefinitions.Add(entityDef.Key, entityDef);
            }


            // Now load Object Pools
            IEnumerable<Type> objectTypes = assembly.GetTypesWithAttribute(typeof(vxRegisterObjectPoolAttribute));

            // Organise Sandbox Items into Categories and Sub Category Lists
            foreach (var type in objectTypes)
            {
                var attr = type.GetCustomAttribute<vxRegisterObjectPoolAttribute>();

                var key = type.Name;


                // Particle Definition
                if (vxEntityRegister.ObjectPools.ContainsKey(type))
                {
                    vxConsole.WriteError(string.Format("ERROR LOADING PARTICLES: Particle '{0}' is using key '{1}' which already exists in the pool.", type.Name, key.ToString()));
                }
                else
                {
                    vxEntityRegister.ObjectPools.Add(type, new vxObjectPoolDefinition(type, attr.PoolSize));
                }
            }




            //now load particles
            RegisterAssemblyParticleSystems(assembly);
        }
    }
}
