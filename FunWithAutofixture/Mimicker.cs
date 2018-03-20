using Newtonsoft.Json;

namespace FunWithAutofixture
{
    public class Mimicker
    {
        public T Echo<T>(T value)
        {
            return value;
        }

        public T CopyCat<T>(T value) where T : class
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            var cat = JsonConvert.DeserializeObject<T>(jsonValue);
            return cat;
        }
    }
}
