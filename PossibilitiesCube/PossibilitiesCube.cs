using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevelopmentSimplyPut.CommonUtilities
{
    public class PossibilitiesCube
    {
        #region Properties
        public Func<Int64[], bool> AttributesCombinationValidator { set; get; }
        public Func<Int64[], bool> InstancesCombinationValidator { set; get; }
        public Func<Int64[], Int64[,], bool> FinalCombinationValidator { set; get; }
        public Int64 InstancesCombinationsMaxRowIndex
        {
            get
            {
                return instancesCombinations.GetLength(0) - 1;
            }
        }
        public Int64 InstancesCombinationsMaxColumnIndex
        {
            get
            {
                return instancesCombinations.GetLength(1) - 1;
            }
        }
        public Int64 AttributesCombinationsMaxRowIndex
        {
            get
            {
                return attributesCombinations.GetLength(0) - 1;
            }
        }
        public Int64 AttributesCombinationsMaxColumnIndex
        {
            get
            {
                return attributesCombinations.GetLength(1) - 1;
            }
        }

        bool combinationsExist;
        public bool CombinationsExist
        {
            get
            {
                return combinationsExist;
            }
        }
        #endregion Properties

        #region Fields
        Int64 numberOfInstances;
        Int64 instancesCombinationsMaxRowIndex;
        Int64 instancesCombinationsMaxColumnIndex;
        Int64 attributesCombinationsMaxRowIndex;
        Int64 attributesCombinationsMaxColumnIndex;
        Int64[,] attributesCombinations;
        Int64[,] instancesCombinations;
        Int64[] attributesPoolsSizes;
        #endregion

        #region Indexers
        public Int64 this[Int64 instancesCombinationIndex, Int64 instanceIndex, Int64 attributeIndex]
        {
            get
            {
                return GetAttributesCombination(instancesCombinations[instancesCombinationIndex, instanceIndex])[attributeIndex];
            }
        }
        public Int64[] this[Int64 instancesCombinationIndex, Int64 instanceIndex]
        {
            get
            {
                return GetAttributesCombination(instancesCombinations[instancesCombinationIndex, instanceIndex]);
            }
        }
        public Int64[] this[Int64 instancesCombinationIndex]
        {
            get
            {
                Int64[] result = new Int64[instancesCombinations.GetLength(1)];
                for (Int64 i = 0; i <= instancesCombinations.GetLength(1); i++)
                {
                    result[i] = instancesCombinations[instancesCombinationIndex, i];
                }
                return result;
            }
        }
        #endregion

        #region Constructors
        public PossibilitiesCube(Int64 _numberOfInstances, params Int64[] _attributesPoolsSizes)
        {
            if (_numberOfInstances <= 0)
            {
                throw new Exception("NumberOfInstancesPerPossibility must be +ve and greater than 0.");
            }

            numberOfInstances = _numberOfInstances;
            attributesPoolsSizes = _attributesPoolsSizes;

            attributesCombinationsMaxRowIndex = 1;
            foreach (Int64 size in _attributesPoolsSizes)
            {
                attributesCombinationsMaxRowIndex *= size;
            }
            
            attributesCombinationsMaxRowIndex--;
            attributesCombinationsMaxColumnIndex = _attributesPoolsSizes.Length - 1;
        }
        #endregion Constructors

        #region Methods
        public Int64[] GetAttributesCombination(Int64 index)
        {
            Int64[] result = new Int64[attributesCombinations.GetLength(1)];

            for (Int64 i = 0; i < attributesCombinations.GetLength(1); i++)
            {
                result[i] = attributesCombinations[index, i];
            }

            return result;
        }
        private void GetPossibilities()
        {
            Int64[,] result = new Int64[instancesCombinationsMaxRowIndex + 1, instancesCombinationsMaxColumnIndex + 1];
            Int64 numberOfFilteredOutPossibilities = 0;

            for (Int64 i = 0; i <= instancesCombinationsMaxRowIndex; i++)
            {
                Int64[] rowResults = GetPossiblityByIndex(i, instancesCombinationsMaxRowIndex, instancesCombinationsMaxColumnIndex, InstancesCombinationValidator, OperationMode.Instances);

                if (rowResults[0] == -1)
                {
                    numberOfFilteredOutPossibilities++;
                }
                else if(null != FinalCombinationValidator)
                {
                    if(!FinalCombinationValidator(rowResults, attributesCombinations))
                    {
                        rowResults[0] = -1;
                        numberOfFilteredOutPossibilities++;
                    }
                }

                for (Int64 k = 0; k < rowResults.Length; k++)
                {
                    result[i, k] = rowResults[k];
                }
            }

            Int64[,] finalResult;
            Int64 actualNumberOfPossibilities = instancesCombinationsMaxRowIndex + 1 - numberOfFilteredOutPossibilities;

            if (actualNumberOfPossibilities > 0)
            {
                finalResult = new Int64[actualNumberOfPossibilities, instancesCombinationsMaxColumnIndex + 1];

                Int64 actualRowIndex = 0;
                for (Int64 i = 0; i < instancesCombinationsMaxRowIndex + 1; i++)
                {
                    if (result[i, 0] != -1)
                    {
                        for (Int64 k = 0; k < instancesCombinationsMaxColumnIndex + 1; k++)
                        {
                            finalResult[actualRowIndex, k] = result[i, k];
                        }

                        actualRowIndex++;
                    }
                }

                combinationsExist = true;
            }
            else
            {
                finalResult = new Int64[1, instancesCombinationsMaxColumnIndex + 1];
                for (Int64 k = 0; k < instancesCombinationsMaxColumnIndex + 1; k++)
                {
                    finalResult[0, k] = -1;
                }

                combinationsExist = false;
            }

            instancesCombinations = finalResult;
        }
        public void BuildPossibilitiesMatrix()
        {
            Int64[,] result = new Int64[attributesCombinationsMaxRowIndex + 1, attributesCombinationsMaxColumnIndex + 1];
            Int64 numberOfFilteredOutPossibilities = 0;

            for (Int64 i = 0; i <= attributesCombinationsMaxRowIndex; i++)
            {
                Int64[] rowResults = GetPossiblityByIndex(i, attributesCombinationsMaxRowIndex, attributesCombinationsMaxColumnIndex, AttributesCombinationValidator, OperationMode.Attributes);

                if (rowResults[0] == -1)
                {
                    numberOfFilteredOutPossibilities++;
                }

                for (Int64 k = 0; k < rowResults.Length; k++)
                {
                    result[i, k] = rowResults[k];
                }
            }

            Int64[,] finalResult;
            Int64 actualNumberOfPossibilities = attributesCombinationsMaxRowIndex + 1 - numberOfFilteredOutPossibilities;

            if (actualNumberOfPossibilities > 0)
            {
                finalResult = new Int64[actualNumberOfPossibilities, attributesCombinationsMaxColumnIndex + 1];

                Int64 actualRowIndex = 0;
                for (Int64 i = 0; i < attributesCombinationsMaxRowIndex + 1; i++)
                {
                    if (result[i, 0] != -1)
                    {
                        for (Int64 k = 0; k < attributesCombinationsMaxColumnIndex + 1; k++)
                        {
                            finalResult[actualRowIndex, k] = result[i, k];
                        }

                        actualRowIndex++;
                    }
                }

                instancesCombinationsMaxRowIndex = intPow(actualNumberOfPossibilities, numberOfInstances) - 1;
                instancesCombinationsMaxColumnIndex = numberOfInstances - 1;
            }
            else
            {
                finalResult = new Int64[1, attributesCombinationsMaxColumnIndex + 1];
                for (Int64 k = 0; k < attributesCombinationsMaxColumnIndex + 1; k++)
                {
                    finalResult[0, k] = -1;
                }

                instancesCombinationsMaxRowIndex = 0;
                instancesCombinationsMaxColumnIndex = 0;
            }

            attributesCombinations = finalResult;
            GetPossibilities();
        }
        private Int64[] GetPossiblityByIndex(Int64 rowIndex, Int64 maxRowIndex, Int64 maxColumnIndex, Func<Int64[], bool> validator, OperationMode mode)
        {
            Int64[] result = null;

            if (rowIndex >= 0)
            {
                if (rowIndex <= maxRowIndex)
                {
                    result = new Int64[maxColumnIndex + 1];

                    for (Int64 i = 0; i <= maxColumnIndex; i++)
                    {
                        result[i] = GetPossiblityByIndex(rowIndex, i, maxRowIndex, maxColumnIndex, mode);
                    }

                    if (null != validator)
                    {
                        if (!validator(result))
                        {
                            for (Int64 i = 0; i <= maxColumnIndex; i++)
                            {
                                result[i] = -1;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception(string.Format("rowIndex can not be greater than {0}", maxRowIndex));
                }
            }
            else
            {
                throw new Exception("rowIndex must be +ve or equal to 0.");
            }

            return result;
        }
        private Int64 GetPossiblityByIndex(Int64 rowIndex, Int64 columnIndex, Int64 maxRowIndex, Int64 maxColumnIndex, OperationMode mode)
        {
            Int64 result = 0;

            if (rowIndex >= 0 && columnIndex >= 0)
            {
                if (rowIndex > maxRowIndex)
                {
                    throw new Exception(string.Format("rowIndex can not be greater than {0}", maxRowIndex));
                }
                else if (columnIndex > maxColumnIndex)
                {
                    throw new Exception(string.Format("columnIndex can not be greater than {0}", maxColumnIndex));
                }
                else
                {
                    Int64 numberOfHops = 1;
                    Int64 numOfItems = 1;

                    switch (mode)
                    {
                        case OperationMode.Attributes:
                            numOfItems = attributesPoolsSizes[columnIndex];
                            if (columnIndex == 0)
                            {
                                numberOfHops = 1;
                            }
                            else
                            {
                                numberOfHops = 1;
                                for (Int64 i = 0; i < columnIndex; i++)
                                {
                                    numberOfHops *= attributesPoolsSizes[i];
                                }
                            }
                            break;
                        case OperationMode.Instances:
                            numOfItems = attributesCombinations.GetLength(0);
                            numberOfHops = intPow(numOfItems, columnIndex);
                            break;
                    }

                    result = GetPossiblityByIndex(numOfItems, numberOfHops, rowIndex);
                }
            }
            else
            {
                throw new Exception("rowIndex and columnIndex must be +ve or equal to 0.");
            }

            return result;
        }
        private Int64 GetPossiblityByIndex(Int64 numberOfItems, Int64 numberOfHops, Int64 rowIndex)
        {
            Int64 result = 0;
            result = rowIndex / numberOfHops;
            result = result % numberOfItems;
            return result;
        }
        private Int64 intPow(Int64 a, Int64 b)
        {
            Int64 result = 0;

            if (0 == b)
            {
                result = 1;
            }
            else if (1 == b)
            {
                result = a;
            }
            else
            {
                result = a;
                for (Int64 i = 0; i < b - 1; i++)
                {
                    result *= a;
                }
            }
            
            return result;
        }
        #endregion Methods
    }

    public enum OperationMode
    {
        Attributes = 0,
        Instances = 1
    }
}
