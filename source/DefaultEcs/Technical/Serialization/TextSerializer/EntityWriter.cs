﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DefaultEcs.Serialization;

namespace DefaultEcs.Technical.Serialization.TextSerializer
{
    internal sealed class EntityWriter : IComponentReader
    {
        #region Fields

        private readonly StreamWriterWrapper _writer;
        private readonly Dictionary<Type, string> _types;
        private readonly Dictionary<Entity, int> _entities;
        private readonly Dictionary<(Entity, Type), int> _components;
        private readonly Predicate<Type> _componentFilter;

        private int _entityCount;
        private Entity _currentEntity;

        #endregion

        #region Initialisation

        public EntityWriter(StreamWriterWrapper writer, Dictionary<Type, string> types, Predicate<Type> componentFilter)
        {
            _writer = writer;
            _types = types;
            _entities = new Dictionary<Entity, int>();
            _components = new Dictionary<(Entity, Type), int>();
            _componentFilter = componentFilter;
        }

        #endregion

        #region Methods

        public void Write(IEnumerable<Entity> entities)
        {
            foreach (Entity entity in entities)
            {
                _entities.Add(entity, ++_entityCount);
                _currentEntity = entity;

                _writer.Stream.WriteLine();
                string entry = entity.IsEnabled() ? nameof(EntryType.Entity) : nameof(EntryType.DisabledEntity);
                _writer.Stream.WriteLine($"{entry} {_entityCount}");

                entity.ReadAllComponents(this);
            }

            foreach (KeyValuePair<Entity, int> pair in _entities)
            {
                foreach (Entity child in pair.Key.GetChildren())
                {
                    if (_entities.TryGetValue(child, out int childId))
                    {
                        _writer.Stream.WriteLine($"{nameof(EntryType.ParentChild)} {pair.Value} {childId}");
                    }
                }
            }
        }

        [SuppressMessage("Performance", "RCS1242:Do not pass non-read-only struct by read-only reference.")]
        public void WriteComponent<T>(in T component, in Entity componentOwner)
        {
            if (!_types.TryGetValue(typeof(T), out _))
            {
                string typeName = typeof(T).Name;

                int repeatCount = 1;
                while (_types.ContainsValue(typeName))
                {
                    typeName = $"{typeof(T).Name}_{repeatCount++}";
                }

                _types.Add(typeof(T), typeName);

                _writer.Stream.WriteLine($"{nameof(EntryType.ComponentType)} {typeName} {TypeNames.Get(typeof(T))}");
            }

            (Entity, Type) componentKey = (componentOwner, typeof(T));
            if (_components.TryGetValue(componentKey, out int key))
            {
                string entry = _currentEntity.IsEnabled<T>() ? nameof(EntryType.ComponentSameAs) : nameof(EntryType.DisabledComponentSameAs);
                _writer.Stream.WriteLine($"{entry} {_types[typeof(T)]} {key}");
            }
            else
            {
                _components.Add(componentKey, _entityCount);
                string entry = _currentEntity.IsEnabled<T>() ? nameof(EntryType.Component) : nameof(EntryType.DisabledComponent);
                _writer.Stream.Write($"{entry} {_types[typeof(T)]} ");
                Converter<T>.Write(_writer, component);
            }
        }

        #endregion

        #region IComponentReader

        void IComponentReader.OnRead<T>(ref T component, in Entity componentOwner)
        {
            if (_componentFilter(typeof(T)))
            {
                Action<EntityWriter, T, Entity> action = _writer.Context?.GetEntityWrite<T>();
                if (action is null)
                {
                    WriteComponent(component, componentOwner);
                }
                else
                {
                    action(this, component, componentOwner);
                }
            }
        }

        #endregion
    }
}
