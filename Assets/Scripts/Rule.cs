internal abstract class Rule
{
    protected int priority;

    public int Priority { get { return priority; } }

    public abstract bool match();
    public abstract void fire();
}