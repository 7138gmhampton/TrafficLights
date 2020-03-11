internal abstract class Rule
{
    protected int priority;
    protected int xPosition;
    protected int yPosition;

    public int Priority { get { return priority; } }

    public abstract bool match(params object[] args);
    public abstract void fire();
}