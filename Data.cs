using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GoldenExport
{
    public class Data
    {
        public static ObservableCollection<Member> memberData =new ObservableCollection<Member>();
        public static ObservableCollection<MemberChoice> memberDataChoice = new ObservableCollection<MemberChoice>();

        public static void Clear()
        {
            Data.memberData.Clear();
        }
        public static void Add(Member member) 
        {
            Data.memberData.Add(member);
        }
        public static void Remove(Member member) 
        {
            
            Data.memberData.Remove(member);
        }

        public static void ClearChoice()
        {
            Data.memberDataChoice.Clear();
        }
        public static void AddChoice(MemberChoice member)
        {
            Data.memberDataChoice.Add(member);
        }
        public static void RemoveChoice(string id)
        {
            MemberChoice member = Data.memberDataChoice.ToList<MemberChoice>().Find(t => t.IDChoice == id);
            Data.memberDataChoice.Remove(member);
        }

    }
}
