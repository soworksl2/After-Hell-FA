namespace AfterHellFA.InputActions{

    public static class GameInputActionsManager
    {
        private readonly static GameInputActions _gameInputActions;

        public static GameInputActions CurrentGameInputActions => _gameInputActions;

        static GameInputActionsManager(){
            _gameInputActions = new GameInputActions();

            _gameInputActions.Enable();
        }
    }

}