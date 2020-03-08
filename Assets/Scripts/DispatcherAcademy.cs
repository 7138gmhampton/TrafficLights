using MLAgents;

public class DispatcherAcademy : Academy
{
    public EnvironmentManager environment;
    public CarManager carManager;
    public MetricsWatcher metricsWatcher;
    public AllLightsController lightsController;

    public override void InitializeAcademy()
    {
        environment.setupEnvironment();
        lightsController.setupJunctionControllers();
        environment.prepareCarManager();
    }
}
