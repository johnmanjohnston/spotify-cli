namespace spotify_cli_cs.AdditionalData
{
    public static class AdditionalData 
    {
        /*
         * "key-value" like data structure to validate
         * data. also we use hexadecimal because
         * hex looks really fuckin cool 
        */
        public enum DataMap
        {
            NULL        = 0x0,

            PLAYLIST    = 0xa,
            ALBUM       = 0x1,
        };
    }
}
