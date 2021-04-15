using System.Collections;
using System.Threading;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public Texture2D NoiseTex;
    public Color[] Pix;
    public Renderer Rend;
    public GameObject Parent;
    public Biomes Cube;
    public Vector3 StartPos;
    private Vector3 _worldSize;
    public Vector3 WorldSize;
    public float MaxX;
    public float MaxY;
    public int MaxStep;
    public WorldLogic WorldManager;
    
    public IEnumerator Walk()
    {
        int i = 0;
        
        //Check if max step is not reached
        while (i >= MaxStep || i <= MaxStep)
        {

            Vector3 _targetPosition = GetPosition();
            
            if (this.transform.position.x >= StartPos.x + MaxX || this.transform.position.y >= StartPos.y + MaxY)
            {
                i++;
                WorldManager.StepPlus();

                if (_targetPosition.x > _worldSize.x || _targetPosition.x < 3 ||
                    _targetPosition.y > _worldSize.y || _targetPosition.y < 3)
                {
                    i++;
                    WorldManager.StepPlus();
                    yield return new WaitForSeconds(0.00001f);
                }

                yield return new WaitForSeconds(0.00001f);
            }
                
            if (CanMoveToPosition(_targetPosition))
            {
                i++;
                WorldManager.BlockPlus();
                WorldManager.StepPlus();
                WorldManager.CheckForStop();
                this.transform.position = _targetPosition;
                GameObject _block = Instantiate(Cube.Prefab, this.transform.position, Quaternion.identity);
                _block.transform.SetParent(Parent.transform);
                WorldManager._positionsDictionary.Add(this.transform.position, Cube.BiomeType);
                yield return new WaitForSeconds(0.00001f);
            }
            else
            {
                WorldManager.StepPlus();
                i++;
                this.transform.position = _targetPosition;
                yield return new WaitForSeconds(0.00001f);
            }
        }

        //}
        //_worldManager.WalkerFinished();
    }
    
    public void WalkFast()
    {
        for (int i = 0; i < MaxStep; i++)
        {
            
            Vector3 _targetPosition = GetPosition();
            //Debug.Log(_targetPosition);
            if (CanMoveToPosition(_targetPosition))
            {    
                WorldManager.CheckForStop();
                WorldManager.BlockPlus();
                WorldManager.StepPlus();
                this.transform.position = _targetPosition;
                GameObject _block = Instantiate(Cube.Prefab, this.transform.position, Quaternion.identity);
                _block.transform.SetParent(Parent.transform);
                 Debug.Log(this.transform.position);
                 WorldManager._positionsDictionary.Add(this.transform.position, Cube.BiomeType);
            }
            else
            {
                WorldManager.StepPlus();
                this.transform.position = _targetPosition;
            }
        }
        //_worldManager.WalkerFinished();
    }

    
    private Vector3 GetPosition()
    {
        Vector3 targetPosition = this.transform.position + dir();
        return targetPosition;
    }
    
    
    /// <summary>
    /// Check for positition is available
    /// </summary>
    /// <param name="_targetPosition"></param>
    /// <returns></returns>
    private bool CanMoveToPosition(Vector3 _targetPosition)
    {
        bool _canMoveToPosition = false;
        BiomeTypes _go;

        if (!WorldManager._positionsDictionary.TryGetValue(_targetPosition, out _go))
        {
            _canMoveToPosition = true;
        }
        
        return _canMoveToPosition;
    }

    private Vector3 dir()
    {
        //0 up
        //1 down
        //2 right
        //3 left

        int _directionInt = Random.Range(0, 4);
        Vector3 _direction = new Vector3();
        switch (_directionInt)
        {
            case 0:
                _direction = new Vector3(1, 0,0);
                break;
            case 1:
                _direction = new Vector3(-1, 0,0);
                break;
            case 2:
                _direction = new Vector3(0, 1,0);
                break;
            case 3:
                _direction = new Vector3(0, -1,0);
                break;
        }
        return _direction;
    }
}
