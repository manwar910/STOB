using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using STOBDataLayer.Interfaces;

namespace STOBDataLayer.Repositories.BoilerPlateDefaults
{
    /// <summary>
    /// Abstract implementation of <see cref="IGenericRepository{TDatabaseEntity}"/>
    /// </summary>
    /// <typeparam name="TContext">Database context type param of type <see cref="DbContext"/></typeparam>
    /// <typeparam name="TDatabaseEntity">Database entity type param of type <see cref="class"/></typeparam>
    /// <typeparam name="TModelEntity">Model entity type param of type <see cref="class"/></typeparam>
    public abstract class GenericRepository<TContext, TDatabaseEntity, TModelEntity> : IGenericRepository<TModelEntity>
        where TDatabaseEntity : class
        where TModelEntity : class
        where TContext : DbContext, new()
    {
        /// <summary>
        /// Database context
        /// </summary>
        protected readonly TContext Entities = new TContext();

        /// <summary>
        /// Object for thread locking
        /// </summary>
        private static readonly object Lock = new object();


        #region Public Methods

        /// <summary>
        /// Abstract definition of converstion function for type <see cref="TDatabaseEntity"/> DB object to type <see cref="TModelEntity"/> model object
        /// </summary>
        /// <param name="entity">Entity to convert of type <see cref="TDatabaseEntity"/></param>
        /// <returns>Converted model entity of type <see cref="TModelEntity"/></returns>
        public abstract TModelEntity ConvertToModel(TDatabaseEntity entity);

        /// <summary>
        /// Abstract definition of converstion function for type <see cref="TModelEntity"/> model object to type <see cref="TDatabaseEntity"/> DB object
        /// </summary>
        /// <param name="entity">Entity to convert of type <see cref="TModelEntity"/></param>
        /// <returns>Converted DB entity of type <see cref="TDatabaseEntity"/></returns>
        public abstract TDatabaseEntity ConvertToDbObject(TModelEntity entity);

        /// <summary>
        /// Find the single model by id
        /// </summary>
        /// <param name="id">Integer Id</param>
        /// <returns>Single instance of type <see cref="TModelEntity"/></returns>
        public virtual TModelEntity FindModelById(int id)
        {
            return ConvertToModel(Entities.Set<TDatabaseEntity>().Find(id));
        }

        /// <summary>
        /// Return a list of all models
        /// </summary>
        /// <returns>List of type <see cref="IEnumerable{TO}"/></returns>
        public virtual IEnumerable<TModelEntity> GetAllModels()
        {
            return ConvertToModelList(GetAll());
        }

        /// <summary>
        /// Add an entity of type <see cref="TModelEntity"/>
        /// </summary>
        /// <param name="entity">Entity of type <see cref="TModelEntity"/> to add, passed by ref to be able to return DB ID</param>
        public virtual void AddModel(ref TModelEntity entity)
        {
            lock (Lock)
            {
                var obj = ConvertToDbObject(entity);

                Entities.Set<TDatabaseEntity>().Add(obj);
                Save();

                entity = ConvertToModel(obj);
            }
        }

        /// <summary>
        /// Delete an entity of type <see cref="TModelEntity"/>
        /// </summary>
        /// <param name="entity">Entity of type <see cref="TModelEntity"/> to delete</param>
        public virtual void DeleteModel(TModelEntity entity)
        {
            lock (Lock)
            {
                Entities.Set<TDatabaseEntity>().Remove(ConvertToDbObject(entity));
                Save();
            }
        }

        /// <summary>
        /// Update an entity of type <see cref="TModelEntity"/>
        /// </summary>
        /// <param name="entity">Entity of type <see cref="TModelEntity"/> to update</param>
        /// <param name="id">Integer ID of item to update to do database comparison</param>
        public virtual void EditModel(ref TModelEntity entity, int id)
        {
            lock (Lock)
            {
                var obj = ConvertToDbObject(entity);
                var originalObj = Entities.Set<TDatabaseEntity>().Find(id);

                Entities.Entry(originalObj).CurrentValues.SetValues(obj);
                Entities.Entry(originalObj).State = EntityState.Modified;

                Save();

                entity = ConvertToModel(originalObj);
            }
        }


        #endregion

        #region Protected Methods

        /// <summary>
        /// Getter for a list of <see cref="TModelEntity"/> entities with an expression
        /// </summary>
        /// <param name="predicate">LINQ expression to specify what to find</param>
        /// <returns><see cref="IEnumerable{TO}"/> list of entities found</returns>
        protected virtual IEnumerable<TModelEntity> FindModelsBy(Expression<Func<TDatabaseEntity, bool>> predicate)
        {
            return ConvertToModelList(FindBy(predicate));
        }

        /// <summary>
        /// Save all changes to the DB context
        /// </summary>
        protected void Save()
        {
            Entities.SaveChanges();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Convert a list of <see cref="TDatabaseEntity"/> entities to a list of <see cref="TModelEntity"/> entities
        /// </summary>
        /// <param name="list"><see cref="IEnumerable{T}"/> list to convert</param>
        /// <returns><see cref="IEnumerable{TO}"/> list of models</returns>
        private IEnumerable<TModelEntity> ConvertToModelList(IEnumerable<TDatabaseEntity> list)
        {
            return list.Select(ConvertToModel).ToList();
        }

        /// <summary>
        /// Getter for an <see cref="IEnumerable{T}"/> list of database objects
        /// </summary>
        /// <param name="predicate">LINQ expression</param>
        /// <returns><see cref="IEnumerable{T}"/> list of database entities</returns>
        private IEnumerable<TDatabaseEntity> FindBy(Expression<Func<TDatabaseEntity, bool>> predicate)
        {
            IEnumerable<TDatabaseEntity> enumerable = Entities.Set<TDatabaseEntity>().Where(predicate);
            return enumerable;
        }

        /// <summary>
        /// Getter for all DB entities
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> list of DB entities</returns>
        private IEnumerable<TDatabaseEntity> GetAll()
        {
            IEnumerable<TDatabaseEntity> enumerable = Entities.Set<TDatabaseEntity>();
            return enumerable;
        }

        public int AddAsync(TModelEntity entity)
        {
            throw new NotImplementedException();
        }

        public int UpdateAsync(TModelEntity entity)
        {
            throw new NotImplementedException();
        }
        //public int UpdateAsync(TModelEntity entity, string type)
        //{
        //    throw new NotImplementedException();
        //}

        public int DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
