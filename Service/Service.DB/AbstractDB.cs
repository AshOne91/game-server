using System;
using System.Collections.Generic;
using System.Text;

namespace Service.DB
{
    public abstract class BaseDBClass
    {
        public abstract void Reset();
        public abstract void Copy(BaseDBClass srcDBData);
        public BaseDBClass() { }
    }

    /*public abstract class BaseDBSlot
    {
        public short _nSlot;
        public bool _isChanged;
        public bool _isDeleted;
        public abstract void Reset();
        public abstract void Copy(object srcData);
        public abstract object GetDBData();
        public BaseDBSlot() { }
    }*/

    /*public abstract class BaseDB
    {
        public bool _isChanged;
        public abstract void Reset();
        public abstract void Copy(object srcData);
        public abstract object GetDBData();
        public BaseDB() { }
    }*/

    public class DBSlot<T> where T : BaseDBClass, new()
    {
        public short _nSlot;
        public bool _isChanged;
        public bool _isDeleted;
        public T _DBData;

        public DBSlot()
        {
            _DBData = new T();
            Reset();
        }

        ~DBSlot()
        {
            Reset();
        }

        public void Reset()
        {
            _nSlot = 0;
            _isChanged = false;
            _isDeleted = false;
            _DBData.Reset();
        }
        public void Copy(T srcData)
        {
            _DBData.Copy(srcData);
        }
    }

    public class DBSlotContainer<U, T> where U : DBSlot<T>, new() where T : BaseDBClass, new()
    {
        private Dictionary<short, U> _DBSlotByIndex;
        private bool _isChanged;

        public DBSlotContainer()
        {
            _DBSlotByIndex = new Dictionary<short, U>();
            _isChanged = false;
        }
        ~DBSlotContainer()
        {
            Reset();
        }
        public void Reset()
        {
            _DBSlotByIndex.Clear();
            _isChanged = false;
        }
        public void EraseAll()
        {
            foreach (var pair in _DBSlotByIndex)
            {
                pair.Value._isChanged = true;
                pair.Value._isDeleted = true;
            }
            _isChanged = true;
        }
        public short GetVacantSlot(bool IsReuse = true)
        {
            for (short slot = 1; slot <= _DBSlotByIndex.Count + 1; ++slot)
            {
                if (_DBSlotByIndex.ContainsKey(slot) == false)
                {
                    return slot;
                }
                else if (IsReuse)
                {
                    U tempSlot = _DBSlotByIndex[slot];
                    if (tempSlot._isDeleted)
                    {
                        return slot;
                    }
                }
            }
            return (short)_DBSlotByIndex.Count;
        }
        public void Copy(DBSlotContainer<U, T> srcContainer, bool isChanged)
        {
            Reset();

            bool IsDestChanged = false;
            if (isChanged && srcContainer.IsChanged())
            {
                IsDestChanged = true;
            }
            SetChanged(IsDestChanged);

            if (!isChanged || srcContainer.IsChanged())
            {
                foreach (var pair in srcContainer._DBSlotByIndex)
                {
                    U srcSlot = pair.Value;

                    if (!isChanged || srcSlot._isChanged)
                    {
                        U newSlot = Insert(srcSlot._nSlot, srcSlot._isChanged, srcSlot._isDeleted);
                        newSlot.Copy(srcSlot._DBData);

                        srcSlot._isChanged = false;
                    }
                }
                srcContainer.SetChanged(false);
            }
        }
        public U Insert(short nSlot, bool isChanged = true, bool isDeleted = false)
        {
            U newSlot = null;
            if (_DBSlotByIndex.ContainsKey(nSlot) == true)
            {
                newSlot = _DBSlotByIndex[nSlot];
                newSlot.Reset();
            }
            else
            {
                newSlot = new U();
                newSlot._nSlot = nSlot;
                _DBSlotByIndex.Add(nSlot, newSlot);
            }
            newSlot._isChanged = isChanged;
            newSlot._isDeleted = isDeleted;
            SetChanged(isChanged);

            return newSlot;
        }
        public bool Erase(short nSlot)
        {
            if (_DBSlotByIndex.ContainsKey(nSlot) == false)
            {
                return false;
            }

            U slot = _DBSlotByIndex[nSlot];
            slot._isChanged = true;
            slot._isDeleted = true;
            SetChanged(true);

            return true;
        }
        public U GetWriteData(short nSlot)
        {
            if (_DBSlotByIndex.ContainsKey(nSlot) == false)
            {
                return null;
            }

            U slot = _DBSlotByIndex[nSlot];
            if (slot._isDeleted == true)
            {
                return null;

            }
            SetChanged(true);

            return slot;
        }
        public U GetReadData(short nSlot)
        {
            if (_DBSlotByIndex.ContainsKey(nSlot) == false)
            {
                return null;
            }

            U slot = _DBSlotByIndex[nSlot];
            if (slot._isDeleted == true)
            {
                return null;
            }

            return slot;
        }
        public void ForEach(Action<U> func, bool isAll = false)
        {
            foreach (var pair in _DBSlotByIndex)
            {
                U slot = pair.Value;
                if (!slot._isDeleted || isAll)
                {
                    func(slot);
                }
            }
        }
        public void BreakableForEach(Func<U, bool> func)
        {
            foreach (var pair in _DBSlotByIndex)
            {
                U slot = pair.Value;
                if (!slot._isDeleted)
                {
                    if (!func(slot))
                    {
                        break;
                    }
                }
            }
        }
        public U Find(Func<U, bool> func)
        {
            foreach (var pair in _DBSlotByIndex)
            {
                U slot = pair.Value;
                if (!slot._isDeleted)
                {
                    if (func(slot))
                    {
                        return slot;
                    }
                }
            }
            return null;
        }
        public List<U> FindAll(Func<U, bool> func)
        {
            List<U> slots = new List<U>();
            foreach (var pair in _DBSlotByIndex)
            {
                U slot = pair.Value;
                if (!slot._isDeleted)
                {
                    slots.Add(slot);
                }
            }
            return slots;
        }
        public bool IsExist(Func<U, bool> func)
        {
            foreach (var pair in _DBSlotByIndex)
            {
                U slot = pair.Value;
                if (!slot._isDeleted)
                {
                    if (func(slot))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public int GetCount()
        {
            int count = 0;
            foreach (var pair in _DBSlotByIndex)
            {
                U slot = pair.Value;
                if (!slot._isDeleted)
                {
                    ++count;
                }
            }
            return count;
        }
        public bool IsChanged() { return _isChanged; }
        public void SetChanged(bool isChanged) { _isChanged = isChanged; }
    }

    public class DBBase<T> where T : BaseDBClass, new()
    {
        public bool _isChanged;
        public T _DBData;

        public DBBase()
        {
            _DBData = new T();
            Reset();
        }

        ~DBBase()
        {
            Reset();
        }

        public void Reset()
        {
            _isChanged = false;
            _DBData.Reset();
        }
    }

    public class DBBaseContainer<U, T> where U : DBBase<T>, new() where T : BaseDBClass, new()
    {
        private U _DBBase;

        public DBBaseContainer()
        {
            _DBBase = new U();
        }
        ~DBBaseContainer()
        {
        }

        public void Reset()
        {
            _DBBase.Reset();
        }

        public void Copy(DBBaseContainer<U, T> srcContainer, bool isChanged)
        {
            Reset();

            _DBBase._isChanged = srcContainer._DBBase._isChanged;

            if (!isChanged || srcContainer._DBBase._isChanged)
            {
                _DBBase._DBData.Copy(srcContainer._DBBase._DBData);

                srcContainer._DBBase._isChanged = false;
            }
        }
        public U GetWriteData(bool isChanged = true)
        {
            _DBBase._isChanged = isChanged;
            return _DBBase;
        }
        public U GetReadData()
        {
            return _DBBase;
        }
    }
}
