using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Net
{
    public abstract class BaseDBClass
    {
        public abstract void Reset();
        public BaseDBClass() { }
    }

    public abstract class BaseDBSlot
    {
        public short _nSlot;
        public bool _isChanged;
        public bool _isDeleted;
        public abstract void Reset();
        public abstract void Copy(object srcData);
        public abstract object GetDBData();
        public BaseDBSlot() { }
    }
    public abstract class BaseDB
    {
        public bool _isChanged;
        public abstract void Reset();
        public abstract void Copy(object srcData);
        public abstract object GetDBData();
        public BaseDB() { }
    }
    public class DBSlot<T> : BaseDBSlot where T : BaseDBClass
    {
        public T _DBData;

        public DBSlot()
        {
            Reset();
        }

        ~DBSlot()
        {
            Reset();
        }

        public override void Reset()
        {
            _nSlot = 0;
            _isChanged = false;
            _isDeleted = false;
            _DBData.Reset();
        }
        public override void Copy(object srcData)
        {
            _DBData = (T)srcData;
        }
        public override object GetDBData()
        {
            return _DBData;
        }
    }

    public class DBSlotContainer<T> where T : BaseDBSlot, new()
    {
        private Dictionary<short, T> _DBSlotByIndex;
        private bool _isChanged;

        public DBSlotContainer()
        {
            _DBSlotByIndex = new Dictionary<short, T>();
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
                    T tempSlot = _DBSlotByIndex[slot];
                    if (tempSlot._isDeleted)
                    {
                        return slot;
                    }
                }
            }
            return (short)_DBSlotByIndex.Count;
        }
        public void Copy(DBSlotContainer<T> srcContainer, bool isChanged)
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
                    T srcSlot = pair.Value;

                    if (!isChanged || srcSlot._isChanged)
                    {
                        T newSlot = Insert(srcSlot._nSlot, srcSlot._isChanged, srcSlot._isDeleted);
                        newSlot.Copy(srcSlot.GetDBData());

                        srcSlot._isChanged = false;
                    }
                }
                srcContainer.SetChanged(false);
            }
        }
        public T Insert(short nSlot, bool isChanged = true, bool isDeleted = false)
        {
            T newSlot = null;
            if (_DBSlotByIndex.ContainsKey(nSlot) == true)
            {
                newSlot = _DBSlotByIndex[nSlot];
                newSlot.Reset();
            }
            else
            {
                newSlot = new T();
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

            T slot = _DBSlotByIndex[nSlot];
            slot._isChanged = true;
            slot._isDeleted = true;
            SetChanged(true);

            return true;
        }
        public T GetWriteData(short nSlot)
        {
            if (_DBSlotByIndex.ContainsKey(nSlot) == false)
            {
                return null;
            }

            T slot = _DBSlotByIndex[nSlot];
            if (slot._isDeleted == true)
            {
                return null;

            }
            SetChanged(true);

            return slot;
        }
        public T GetReadData(short nSlot)
        {
            if (_DBSlotByIndex.ContainsKey(nSlot) == false)
            {
                return null;
            }

            T slot = _DBSlotByIndex[nSlot];
            if (slot._isDeleted == true)
            {
                return null;
            }

            return slot;
        }
        public void ForEach(Action<T> func, bool isAll = false)
        {
            foreach (var pair in _DBSlotByIndex)
            {
                T slot = pair.Value;
                if (!slot._isDeleted || isAll)
                {
                    func(slot);
                }
            }
        }
        public void BreakableForEach(Func<T, bool> func)
        {
            foreach (var pair in _DBSlotByIndex)
            {
                T slot = pair.Value;
                if (!slot._isDeleted)
                {
                    if (!func(slot))
                    {
                        break;
                    }
                }
            }
        }
        public T Find(Func<T, bool> func)
        {
            foreach (var pair in _DBSlotByIndex)
            {
                T slot = pair.Value;
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
        public List<T> FindAll(Func<T, bool> func)
        {
            List<T> slots = new List<T>();
            foreach (var pair in _DBSlotByIndex)
            {
                T slot = pair.Value;
                if (!slot._isDeleted)
                {
                    slots.Add(slot);
                }
            }
            return slots;
        }
        public bool IsExist(Func<T, bool> func)
        {
            foreach (var pair in _DBSlotByIndex)
            {
                T slot = pair.Value;
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
                T slot = pair.Value;
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

    public class DBBase<T> : BaseDB where T : BaseDBClass
    {
        public T _DBData;

        public DBBase()
        {
            Reset();
        }

        ~DBBase()
        {
            Reset();
        }

        public override void Reset()
        {
            _isChanged = false;
            _DBData.Reset();
        }

        public override void Copy(object srcData)
        {
            _DBData = (T)srcData;
        }

        public override object GetDBData()
        {
            return _DBData;
        }
    }

    public class DBBaseContainer<T> where T : BaseDB, new()
    {
        private T _DBBase;

        public DBBaseContainer()
        {
            _DBBase = new T();
        }
        ~DBBaseContainer()
        {
        }

        public void Reset()
        {
            _DBBase.Reset();
        }

        void Copy(DBBaseContainer<T> srcContainer, bool isChanged)
        {
            Reset();

            _DBBase._isChanged = srcContainer._DBBase._isChanged;

            if (!isChanged || srcContainer._DBBase._isChanged)
            {
                _DBBase.Copy((object)_DBBase.GetDBData());

                srcContainer._DBBase._isChanged = false;
            }
        }
        T GetWriteData(bool isChanged = true)
        {
            _DBBase._isChanged = isChanged;
            return _DBBase;
        }
        T GetReadData()
        {
            return _DBBase;
        }
    }
}
