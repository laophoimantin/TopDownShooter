using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : Singleton<UpdateManager>
{
    private List<IUpdater> _listUpdater = new List<IUpdater>();
    private List<IUpdater> _tmpListUpdater = new List<IUpdater>();

    private List<IFixedUpdater> _listFixedUpdater = new List<IFixedUpdater>();
    private List<IFixedUpdater> _tmpListFixedUpdater = new List<IFixedUpdater>();

    private bool _canUpdate = true;
    
    void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }
    
    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        _canUpdate = (newState == GameManager.GameState.Gameplay);
    }
    
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
        if (!_canUpdate) return;
        if (_listUpdater.Count == 0) return;

        for (int i = _listUpdater.Count - 1; i >= 0; i--)
        {
            _listUpdater[i].OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (!_canUpdate) return;
        if (_listFixedUpdater.Count == 0) return;
        
        for (int i = _listFixedUpdater.Count - 1; i >= 0; i--)
        {
            _listFixedUpdater[i].OnFixedUpdate();
        }
    }
}