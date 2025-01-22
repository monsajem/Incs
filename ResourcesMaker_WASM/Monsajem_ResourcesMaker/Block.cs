
using System;
using System.Collections.Generic;
using System.Text;

namespace Monsajem_LanguageCompiler
{

    public class Block
    {
        public static Block CurrentBlock;

        private string Code;

        public List<Block> Codes = new List<Block>();

        private static StringBuilder Compiled;

        private static string Tabs(int Tabs)
        {
            string text = "";
            for (int i = 0; i < Tabs; i++)
            {
                text += "\t";
            }

            return text;
        }

        private void _Compile(int InnerPosition = 0)
        {
            if (InnerPosition == 0)
            {
                Compiled = new StringBuilder();
            }

            if (Code != null)
            {
                Compiled.Append("\n" + Tabs(InnerPosition - 1) + Code);
            }

            foreach (Block code in Codes)
            {
                code._Compile(InnerPosition + 1);
            }
        }

        public string Compile()
        {
            _Compile();
            string result = Compiled.ToString();
            Compiled.Clear();
            Compiled = null;
            return result;
        }

        public void NewBlock(Action<Block> Maker)
        {
            Block block = new Block();
            Block currentBlock = CurrentBlock;
            CurrentBlock = block;
            Maker(block);
            CurrentBlock = currentBlock;
            Codes.Add(block);
        }

        public void NewBlock(Action Maker)
        {
            NewBlock((Action<Block>)delegate
            {
                Maker();
            });
        }

        public void NewBlock(string Block)
        {
            Block block = new Block();
            block.Code = Block;
            Codes.Add(block);
        }

        public static Block operator +(Block Block, string Code)
        {
            Block.NewBlock(Code);
            return Block;
        }
    }
}


