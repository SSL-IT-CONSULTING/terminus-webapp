alter proc spClosePreviousBill
			@billId uniqueIdentifier
as

declare @amount decimal(18,4)
declare @propertyDirectoryId uniqueIdentifier
declare @monthYear nvarchar(8)

select	@amount = sum(t1.amount), 
		@propertyDirectoryId = t0.propertyDirectoryId,
		@monthYear = t0.MonthYear
from	Billings t0 with (nolock)
inner join BillingLineItems t1 with (nolock) on t0.billId=t1.billingId
where t0.billId=@billId 
and t0.billType='MB'
and t1.billLineType in ('MONTHLYBILLITEM_PREVBAL','MONTHLYASSOCDUE_PREVBAL')
group by t0.propertyDirectoryId, t0.MonthYear

if(@amount>0 and @propertyDirectoryId is not null)
begin
	declare @prevBillId uniqueidentifier
	declare @balance decimal(18,4)

	select top 1 @prevBillId = billId from Billings t0 with (nolock) 
	where t0.propertyDirectoryId = @propertyDirectoryId
	and t0.billType='MB'
	and t0.MonthYear<@monthYear

	select @balance = sum(amount-amountPaid) from BillingLineItems with (nolock) where billingId=@billId
	select '[' + name +'],' from syscolumns where id=object_id('BillingLineItems') order by colorder

	insert into BillingLineItems
	(
		[Id],
		[description],
		[amount],
		[lineNo],
		[billingId],
		[generated],
		[amountPaid],
		[billLineType]
	)
	select top 1 newid(),
			'Close Bill',
			@balance * -1,
			[lineNo]+1,
			[billingId],
			[generated],
			0,
			'MONTHLYBILLITEMCLOSEBILL'
	from BillingLineItems with (nolock) where billingId=@prevBillId
	order by [lineNo] desc


	update t0
	set t0.totalAmount = t1.totalAmount,
		t0.balance = t1.Balance
	from Billings t0
	inner join (select	billingId, 
						sum(amount) as totalAmount, 
						sum(amount - amountPaid) as Balance 
				from	BillingLineItems with (nolock) 
				where billingId=@prevBillId
	group by billingId) t1 on t0.billId=t1.billingId
	where t0.billId=@prevBillId

end