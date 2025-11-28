namespace a_Scripts
{
    public class Test
    {

        public void Foo1(out int a, out int b)
        {
            a = 1;
            b = 1;
        }

        public void output()
        {
            int a, b;
            Foo1(out a, out b);
            Foo2(ref a,ref b);
            Foo3(in a, in b);
            
        }
        
        public void Foo2(ref int a, ref int b)
        {
            
        }
        
        public void Foo3(in int a, in int b)
        {
            
        }
    }
}