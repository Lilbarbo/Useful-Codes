using System;
using UnityEngine;


public interface IState
{
    void Enter();
    IState Tick();
    void Exit();
}

public class MaquinaDeEstados
{
    private IState _estadoAtual;

    public void SetState(IState novoEstado)
    {
        _estadoAtual?.Exit();
        _estadoAtual = novoEstado;
        novoEstado?.Enter();
    }

    public void Tick()
    {
        if (_estadoAtual == null)
        {
            return;
        }
        
        var proximoEstado = _estadoAtual.Tick();
        if (proximoEstado != null && proximoEstado != _estadoAtual)
        {
            SetState(proximoEstado);
        }
    }
}

public class PatrolState : IState
{
    public void Enter()
    {
        Debug.Log("Patrol");
    }

    public IState Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return new Chase();
        }
        
        return null;
    }

    public void Exit()
    {
        Debug.Log("Exit Patrol");
    }
}

public class Chase : IState
{
    public void Enter()
    {
        Debug.Log("Chase");
    }

    public IState Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return new PatrolState();
        }
        return null;
    }

    public void Exit()
    {
        Debug.Log("Exit Chase");
    }
}
public class FSMClasses : MonoBehaviour
{
    MaquinaDeEstados maquina = new MaquinaDeEstados();

    private void Awake()
    {
        maquina.SetState(new PatrolState());
    }
    void Update()
    {
        maquina.Tick();
    }
}


