using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CirclesWar.Data;

namespace CirclesWar {
    public class GameController : MonoBehaviour
    {
        GameField gameField;
        GameConfig config;
        CameraController cameraContorller;
        Simulator simulator;

        Transform circlesContainer;

        void Awake()
        {
            var gameConfig = new ConfigLoader().GetConfig();

            InitializeGame(gameConfig);
        }

        private void InitializeGame(GameConfig config)
        {
            this.config = config;

            var gameFieldGameObject = new GameObject("GameField");

            gameField = gameFieldGameObject.AddComponent<GameField>();
            gameField.CreateField(config.gameAreaWidth, config.gameAreaHeight);

            if (cameraContorller == null)
            {
                cameraContorller = Camera.main.gameObject.AddComponent<CameraController>();          
            }
            cameraContorller.SetTargetGameObject(gameFieldGameObject);

            var generator = new CirclesGenerator(config.numUnitsToSpawn, config.unitSpawnMinRadius, config.unitSpawnMaxRadius);

            simulator = new Simulator(config.gameAreaWidth, config.gameAreaHeight);

            var container = new GameObject("Circles");
            circlesContainer = container.transform;

            StartCoroutine("SpawnCircle", generator);
        }

        IEnumerator SpawnCircle(CirclesGenerator generator)
        {
            var circleData = generator.GetNext();
            while (circleData != null)
            {
                var circleObject = new GameObject();
                var circleController = circleObject.AddComponent<Circle>();

                var tryCount = 0;
                var correctPosition = false;
                var circleRadius = circleData.GetRadius();
                while (!correctPosition && tryCount < 10)
                {
                    var positionX = Random.value * (config.gameAreaWidth - 2 * circleRadius) - config.gameAreaWidth * 0.5f + circleRadius;
                    var positionY = Random.value * (config.gameAreaHeight - 2 * circleRadius) - config.gameAreaHeight * 0.5f + circleRadius;

                    circleData.SetPositionX(positionX);
                    circleData.SetPositionY(positionY);
                    correctPosition = simulator.SpawnCircle(circleData);
                }

                if (!correctPosition)
                {
                    Debug.LogError("cant add circle");
                    yield return null;
                }

                circleController.SetData(circleData);
                circleData = generator.GetNext();
                circleObject.transform.SetParent(circlesContainer, true);
                
                yield return new WaitForSeconds(config.unitSpawnDelay * 0.001f);
            }

            simulator.StartMoving(config.unitSpawnMinSpeed, config.unitSpawnMaxSpeed);

            yield return null;
        }
                  
        void FixedUpdate()
        {
            simulator?.Update(Time.fixedDeltaTime);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}