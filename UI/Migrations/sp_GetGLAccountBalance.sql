alter proc dbo.sp_GetGLAccountBalance
				@companyId nvarchar(10)
as
select t0.accountId, t0.accountCode, t0.accountDesc,t0.rowOrder,
sum(t1.amount * case when t1.[type]='C' then -1
else 1 end) balance
from GLAccounts t0
left outer join JournalEntriesDtl t1 on t0.accountId=t1.accountId
left outer join JournalEntriesHdr t2 on t2.id=t1.JournalEntryHdrid
where t0.companyId=@companyId
group by t0.accountId, t0.accountCode, t0.accountDesc, t0.rowOrder
order by t0.rowOrder