﻿using System;

namespace DefaultEcs.System
{
    /// <summary>
    /// Represents a base class to process updates on a given <see cref="EntitySet"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of the object used as state to update the system.</typeparam>
    public abstract class AEntitySetSystem<T> : ASystem<T>
    {
        #region Fields

        private readonly EntitySet _set;

        #endregion

        #region Initialisation

        /// <summary>
        /// Initialise a new instance of the <see cref="AEntitySetSystem{T}"/> class with the given <see cref="EntitySet"/> and <see cref="SystemRunner{T}"/>.
        /// </summary>
        /// <param name="set">The <see cref="EntitySet"/> on which to process the update.</param>
        /// <param name="runner">The <see cref="SystemRunner{T}"/> used to process the update in parallel if not null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="set"/> is null.</exception>
        protected AEntitySetSystem(EntitySet set, SystemRunner<T> runner)
            : base(runner)
        {
            _set = set ?? throw new ArgumentNullException(nameof(set));
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="AEntitySetSystem{T}"/> class with the given <see cref="EntitySet"/>.
        /// </summary>
        /// <param name="set">The <see cref="EntitySet"/> on which to process the update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="set"/> is null.</exception>
        protected AEntitySetSystem(EntitySet set)
            : this(set, null)
        { }

        #endregion

        #region Methods

        /// <summary>
        /// Update the given <see cref="Entity"/> instances once.
        /// </summary>
        /// <param name="state">The state to use.</param>
        /// <param name="entities">The <see cref="Entity"/> instances to update.</param>
        protected virtual void Update(T state, ReadOnlySpan<Entity> entities)
        {
            for (int i = 0; i < entities.Length; ++i)
            {
                Update(state, entities[i]);
            }
        }

        /// <summary>
        /// Update the given <see cref="Entity"/> instance once.
        /// </summary>
        /// <param name="state">The state to use.</param>
        /// <param name="entity">The <see cref="Entity"/> instance to update.</param>
        protected abstract void Update(T state, in Entity entity);

        #endregion

        #region ASystem

        private protected sealed override void DefaultUpdate(T state) => Update(state, _set.GetEntities());

        internal sealed override void Update(T state, int index, int maxIndex)
        {
            ReadOnlySpan<Entity> entities = _set.GetEntities();
            int entitiesToUpdate = entities.Length / (maxIndex + 1);

            Update(state, index == maxIndex ? entities.Slice(index * entitiesToUpdate) : entities.Slice(index * entitiesToUpdate, entitiesToUpdate));
        }

        #endregion
    }
}
