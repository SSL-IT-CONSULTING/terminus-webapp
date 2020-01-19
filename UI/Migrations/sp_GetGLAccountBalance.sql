create proc dbo.sp_GetGLAccountBalance
				@companyId nvarchar(10)
as
select t2.accountId, t2.accountCode, t2.accountDesc,t2.rowOrder,
sum(t1.amount * case when t2.cashAccount=1 and t0.source='expense' then -1
else 1 end) balance
from JournalEntriesHdr t0
inner join JournalEntriesDtl t1 on t0.id=t1.JournalEntryHdrid
inner join GLAccounts t2 on t2.accountId=t1.accountId
where t2.companyId=@companyId
group by t2.accountId, t2.accountCode, t2.accountDesc, t2.rowOrder
order by t2.rowOrder