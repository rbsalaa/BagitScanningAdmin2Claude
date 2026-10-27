using SelfKOrderWeb.DTO;

namespace SelfKOrderWeb.Factory.IFactory
{
    public interface ISubscrubtionInfoRepo
    {
        public Task<IEnumerable<SubscrubtionInfoDTO>> GetSubscrubtionInfoList( );
        public Task<IEnumerable<SubscrubtionInfoDTO>> GetHistorySubscrubtionInfoList();
        public Task<SubscrubtionInfoDTO> GetSubscrubtionInfo(  string id);
        public int DeleteSubscrubtionInfo(  string id);
        public int EditSubscrubtionInfo(  SubscrubtionInfoDTO obj);
        public int SaveSubscrubtionInfo(  SubscrubtionInfoDTO obj);
        public int AddLicensesDetail(SubscrubtionInfoDetailsDTO e);
        
        public  Task<List<SubscrubtionInfoDetailsDTO>> GetSubscrubtionDetailInfo(string id);//GetHestorySubscrubtionDetailInfo
        public Task<List<SubscrubtionInfoDetailsDTO>> GetHestorySubscrubtionDetailInfo(string id);
        public int DeleteSubscrubtionDetailInfo(string id, string subID);
    }
}
