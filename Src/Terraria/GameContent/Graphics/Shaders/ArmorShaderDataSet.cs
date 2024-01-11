﻿
using System.Collections.Generic;
using GameManager;
using GameManager.DataStructures;

namespace GameManager.Graphics.Shaders
{
    public class ArmorShaderDataSet
    {
        protected List<ArmorShaderData> _shaderData = new List<ArmorShaderData>();
        protected Dictionary<int, int> _shaderLookupDictionary = new Dictionary<int, int>();
        protected int _shaderDataCount;

        public T BindShader<T>(int itemId, T shaderData) where T : ArmorShaderData
        {
            _shaderLookupDictionary[itemId] = ++this._shaderDataCount;
            _shaderData.Add(shaderData);
            return shaderData;
        }

        public void Apply(int shaderId, Entity entity, DrawData? drawData = null)
        {
            if (shaderId != 0 && shaderId <= _shaderDataCount)
                _shaderData[shaderId - 1].Apply(entity, drawData);
            else
            { 
                try
                {
                    //Game1.pixelShader.CurrentTechnique.Passes[0].Apply();
                }
                catch { }
            }
        }

        public void ApplySecondary(int shaderId, Entity entity, DrawData? drawData = null)
        {
            if (shaderId != 0 && shaderId <= _shaderDataCount)
                _shaderData[shaderId - 1].GetSecondaryShader(entity).Apply(entity, drawData);
            else
            {
                try
                {
                    //Game1.pixelShader.CurrentTechnique.Passes[0].Apply();
                }
                catch { }
            }
        }

        public ArmorShaderData GetShaderFromItemId(int type)
        {
            if (_shaderLookupDictionary.ContainsKey(type))
                return _shaderData[_shaderLookupDictionary[type] - 1];

            return null;
        }

        public int GetShaderIdFromItemId(int type)
        {
            if (_shaderLookupDictionary.ContainsKey(type))
                return _shaderLookupDictionary[type];

            return 0;
        }

        public ArmorShaderData GetSecondaryShader(int id, Player player)
        {
            if (id != 0 && id <= _shaderDataCount && _shaderData[id - 1] != null)
                return _shaderData[id - 1].GetSecondaryShader(player);

            return null;
        }
    }
}