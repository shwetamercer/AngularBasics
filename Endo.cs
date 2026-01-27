using System;
using System.Collections.Generic;
using System.Linq;

namespace InsuranceEndorsements
{
    /// <summary>
    /// Handles ordering of endorsements according to predefined business rules
    /// </summary>
    public static class EndorsementOrdering
    {
        /// <summary>
        /// Predefined order for Mandatory endorsements (most efficient: Dictionary with order index)
        /// Lower number = higher priority in display order
        /// </summary>
        private static readonly Dictionary<string, int> MandatoryEndorsementOrder = new Dictionary<string, int>
        {
            { "Reinsurance Liability Clauses", 1 },
            { "Statutory Compliance Clause", 2 },
            { "Premium Payment Warranty", 3 },
            { "Policy Territory Restrictions", 4 },
            { "Subrogation Waiver Clause", 5 },
            { "Loss Notification Requirements", 6 },
            { "Cancellation Notice Requirements", 7 },
            { "Insurer's Right of Inspection", 8 },
            { "Currency and Exchange Rate Provisions", 9 },
            { "Arbitration Clause", 10 }
        };

        /// <summary>
        /// Predefined order for Risk Specific endorsements (optional)
        /// </summary>
        private static readonly Dictionary<string, int> RiskSpecificEndorsementOrder = new Dictionary<string, int>
        {
            { "Border Rejection Coverage", 1 },
            { "Cyber Risk Extension", 2 },
            { "Environmental Liability Extension", 3 },
            { "Product Recall Coverage", 4 },
            { "Pandemic Exclusion Clause", 5 },
            { "Catastrophic Event Limitation", 6 }
        };

        /// <summary>
        /// Predefined order for Other endorsements (optional)
        /// </summary>
        private static readonly Dictionary<string, int> OtherEndorsementOrder = new Dictionary<string, int>
        {
            { "Claims Co-operation Clause", 1 },
            { "Extended Reporting Period Option", 2 },
            { "Aggregate Deductible Provision", 3 },
            { "Automatic Coverage Extension", 4 }
        };

        /// <summary>
        /// Get user-selected mandatory endorsements in predefined order
        /// Most efficient: O(n log n) due to sorting
        /// </summary>
        public static List<Endorsement> GetOrderedMandatoryEndorsements(List<Endorsement> userSelection)
        {
            return userSelection
                .Where(e => e.Type == "Mandatory")
                .OrderBy(e => GetOrderIndex(e.Name, MandatoryEndorsementOrder))
                .ToList();
        }

        /// <summary>
        /// Get user-selected risk specific endorsements in predefined order
        /// </summary>
        public static List<Endorsement> GetOrderedRiskSpecificEndorsements(List<Endorsement> userSelection)
        {
            return userSelection
                .Where(e => e.Type == "RiskSpecific")
                .OrderBy(e => GetOrderIndex(e.Name, RiskSpecificEndorsementOrder))
                .ToList();
        }

        /// <summary>
        /// Get user-selected other endorsements in predefined order
        /// </summary>
        public static List<Endorsement> GetOrderedOtherEndorsements(List<Endorsement> userSelection)
        {
            return userSelection
                .Where(e => e.Type == "Other")
                .OrderBy(e => GetOrderIndex(e.Name, OtherEndorsementOrder))
                .ToList();
        }

        /// <summary>
        /// Get all user-selected endorsements ordered by type and predefined order
        /// Returns: Mandatory (ordered) -> RiskSpecific (ordered) -> Other (ordered)
        /// </summary>
        public static List<Endorsement> GetFullyOrderedEndorsements(List<Endorsement> userSelection)
        {
            var orderedList = new List<Endorsement>();

            // Add mandatory endorsements in predefined order
            orderedList.AddRange(GetOrderedMandatoryEndorsements(userSelection));

            // Add risk specific endorsements in predefined order
            orderedList.AddRange(GetOrderedRiskSpecificEndorsements(userSelection));

            // Add other endorsements in predefined order
            orderedList.AddRange(GetOrderedOtherEndorsements(userSelection));

            return orderedList;
        }

        /// <summary>
        /// Helper method to get order index (O(1) lookup)
        /// Returns int.MaxValue for items not in the order dictionary (pushes them to end)
        /// </summary>
        private static int GetOrderIndex(string endorsementName, Dictionary<string, int> orderDict)
        {
            return orderDict.TryGetValue(endorsementName, out int order) ? order : int.MaxValue;
        }

        /// <summary>
        /// Format endorsements for printing/display
        /// </summary>
        public static void PrintOrderedEndorsements(List<Endorsement> endorsements, string title = "Endorsements")
        {
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('-', 70));

            if (endorsements.Count == 0)
            {
                Console.WriteLine("  (No endorsements)");
                return;
            }

            for (int i = 0; i < endorsements.Count; i++)
            {
                var e = endorsements[i];
                Console.WriteLine($"{i + 1,2}. {e.Name,-50} [{e.Type}]");
            }
        }

        /// <summary>
        /// Print user selection in business-defined order (grouped by type)
        /// </summary>
        public static void PrintUserSelectionByTypeOrder(List<Endorsement> userSelection)
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("USER SELECTION - ORDERED BY TYPE AND PRIORITY");
            Console.WriteLine(new string('=', 70));

            // Print Mandatory endorsements
            var orderedMandatory = GetOrderedMandatoryEndorsements(userSelection);
            PrintOrderedEndorsements(orderedMandatory, "\n1. MANDATORY ENDORSEMENTS");

            // Print Risk Specific endorsements
            var orderedRiskSpecific = GetOrderedRiskSpecificEndorsements(userSelection);
            PrintOrderedEndorsements(orderedRiskSpecific, "\n2. RISK SPECIFIC ENDORSEMENTS");

            // Print Other endorsements
            var orderedOther = GetOrderedOtherEndorsements(userSelection);
            PrintOrderedEndorsements(orderedOther, "\n3. OTHER ENDORSEMENTS");

            Console.WriteLine("\n" + new string('=', 70));
        }

        /// <summary>
        /// Alternative: Print as a single flat list in predefined order
        /// </summary>
        public static void PrintUserSelectionFlatOrder(List<Endorsement> userSelection)
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("USER SELECTION - SINGLE ORDERED LIST");
            Console.WriteLine(new string('=', 70));

            var fullyOrdered = GetFullyOrderedEndorsements(userSelection);

            for (int i = 0; i < fullyOrdered.Count; i++)
            {
                var e = fullyOrdered[i];
                Console.WriteLine($"{i + 1,2}. {e.Name,-50} [{e.Type}]");
            }

            Console.WriteLine(new string('=', 70));
        }

        /// <summary>
        /// Performance comparison: Show ordering with execution time
        /// </summary>
        public static void DemonstratePerformance()
        {
            var userSelection = EndorsementData.UserSelectionList;

            Console.WriteLine("\n=== PERFORMANCE DEMONSTRATION ===\n");

            // Method 1: Dictionary-based ordering (Most Efficient)
            var sw1 = System.Diagnostics.Stopwatch.StartNew();
            var ordered1 = GetOrderedMandatoryEndorsements(userSelection);
            sw1.Stop();

            Console.WriteLine($"Method 1 - Dictionary Lookup: {sw1.Elapsed.TotalMilliseconds:F4} ms");
            Console.WriteLine($"  Result count: {ordered1.Count}");
            Console.WriteLine($"  Complexity: O(n log n) due to sorting");

            // Method 2: List-based ordering (Alternative)
            sw1.Restart();
            var ordered2 = OrderByListIndex(userSelection);
            sw1.Stop();

            Console.WriteLine($"\nMethod 2 - List IndexOf: {sw1.Elapsed.TotalMilliseconds:F4} ms");
            Console.WriteLine($"  Result count: {ordered2.Count}");
            Console.WriteLine($"  Complexity: O(n * m) where m is order list size");

            Console.WriteLine("\n✅ Dictionary-based approach is more efficient for large datasets");
        }

        /// <summary>
        /// Alternative ordering method using List.IndexOf (less efficient for large datasets)
        /// </summary>
        private static List<Endorsement> OrderByListIndex(List<Endorsement> userSelection)
        {
            var orderList = new List<string>
            {
                "Reinsurance Liability Clauses",
                "Statutory Compliance Clause",
                "Premium Payment Warranty",
                "Policy Territory Restrictions",
                "Subrogation Waiver Clause",
                "Loss Notification Requirements",
                "Cancellation Notice Requirements",
                "Insurer's Right of Inspection",
                "Currency and Exchange Rate Provisions",
                "Arbitration Clause"
            };

            return userSelection
                .Where(e => e.Type == "Mandatory")
                .OrderBy(e =>
                {
                    int index = orderList.IndexOf(e.Name);
                    return index == -1 ? int.MaxValue : index;
                })
                .ToList();
        }
    }
}
