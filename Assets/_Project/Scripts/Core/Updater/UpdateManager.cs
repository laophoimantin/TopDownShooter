using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : Singleton<UpdateManager>
{
    private List<IUpdater> _listUpdater = new List<IUpdater>();
    private List<IUpdater> _tmpListUpdater = new List<IUpdater>();

    private List<IFixedUpdater> _listFixedUpdater = new List<IFixedUpdater>();
    private List<IFixedUpdater> _tmpListFixedUpdater = new List<IFixedUpdater>();


    public void OnAssignUpdater(IUpdater update)
    {
        if (!_listUpdater.Contains(update))
        {
            _listUpdater.Add(update);
        }
    }

    public void OnUnassignUpdater(IUpdater update)
    {
        _listUpdater.Remove(update);
    }

    public void OnAssignFixedUpdater(IFixedUpdater fixedUpdate)
    {
        if (!_listFixedUpdater.Contains(fixedUpdate))
        {
            _listFixedUpdater.Add(fixedUpdate);
        }
    }

    public void OnUnassignFixedUpdater(IFixedUpdater fixedUpdate)
    {
        _listFixedUpdater.Remove(fixedUpdate);
    }

    private void Update()
    {
        //if (GameManager.Instance.IsOver) return;
        //if (GameManager.Instance.IsPaused) return;
        if (_listUpdater.Count == 0) return;

        _tmpListUpdater = _listUpdater;

        for (int i = 0; i < _tmpListUpdater.Count; i++)
        {
            _tmpListUpdater[i].OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        //if (GameManager.Instance.IsOver) return;
        //if (GameManager.Instance.IsPaused) return;
        if (_listFixedUpdater.Count == 0) return;

        _tmpListFixedUpdater = _listFixedUpdater;

        for (int i = 0; i < _tmpListFixedUpdater.Count; i++)
        {
            _tmpListFixedUpdater[i].OnFixedUpdate();
        }
    }
}