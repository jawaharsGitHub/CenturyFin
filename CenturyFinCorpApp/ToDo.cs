﻿
//Done: Auto Select Next Transaction Date and field.
//Done: Auto Populate - Daily Collection Amount
//Done: Number of days taken to close the transaction.
//Done: Actual Interest got from customer - eg: customer got - 10,000 and got all money from 100 days.then interest got is 100% that is 8% for 100 days.but if customer return all amount in 10 days.
//Done: Start txn date should not be the first collection date, it should be the date the money given
//Done: Update customer creation date
//Done: Collection By Date(may be reports)
//Done: Increase customer name drop down control width in create new txn screen
//Done: Auto Populate interest(8%)
//Done: For Existing CUstomer Disable Name and Phone No.
//Done: For Existing CUstomer Disable Name and Phone No.
//Done: (Removed this feature) Please clarify - how company vs jawahar investment works?
//Done: Have options to show both active and non active customers in customer screen.
//Done: change backup folder name as [ClosedNotes]
//Done: How to see closed Transaction?
// Done: Asc/Desc by date
//Done: Best Customer (Top 3)
//Done: Worst Customers.(Top 10)
//Done: edit transaction.
// Done: Delete customer
//Done: Set focus to txtSearch in customer form.
// Done: Move Daily Txn as seperate file.(did using Batch)
//Done: X-customer - to know the reason why they didn't get money.
// Done: new menu - 1. Politics  2. Health
// Done: Collection Spot

//=====================================================================
// SCENARIOS
//=====================================================================

// 1. Move just balnce from one cus to another cus. --> force close the source cus and keep the interest for destination customer. then add new topup for destimation customer with interest as this source customer interest and make sure to keep the same and correct date for this txns may be we can have a txn details like comments for this top up.)
// REFER - move from one to another Bal. Commit d0941c59
// 2. Move Part of balance from one Cus to another cus.
// 3. Move all txns from one to another cus.
// 4. insert missing or new txns inside existing txns.
// 5. insert missing txns (by mama)




//=====================================================================
//=====================================================================

//TODO: Mobile Message Service
//TODO: Multi Select Date Control
//TODO: Tool - Auto Fill(all entries)
//DOne: While Creating a customer(Existing customer) should warn for the worst customer.
//TODO: Expected Amount the given date(Future Date)
//TODO: Tab-Order
//Done: While adding customer - display the customer no and sequence number eg: if 2nd time given to same customer then it should shows as 10(2nd time.)
//TODO: Enable sorting, paging for customer page.
//TODO: change icon of the project.
//TODO: installer for the project.
//TODO: Add Customer Name in ClosedNotes Folders [ClosedNotes]
//TODO: Sometimes file gets locked (Cant access file in use)
//TODO: Refresh Data Automatically.
//TODO: TxnId may not be correct if last customer closes
//TODO: Handle all exceptions. (Dont show any technical exceptions.should not break the application)
//TODO: please verify all date format.  (only date needed, no time needed)
//TODO: Internal tool to do small modifications.
//TODO: keyboard short cuts for all actions strat wth (Date, amount, add txn)
//TODO: select all amount.
//TODO: Auto Select Existing customer.
//TODO: Show [Sequence No] - No of times 
//TODO: Git Flow for all customers.
// TODO: Esc should close the form.
// Issue: Check customer and total notes count.
// TODO: list who not payed for the given date.
// TODO: AUTO - Collection sheet.
// TODO: Add Run Closed in other places also.
// TODO: While Running Closed batch - update closed date also.
// TODO: InHand have only add (with tet update), so implement update also.
// TODO: Logging activity  (using log4net)
// TODO: Put daily Txn FIle Seperatedly like closed notes.
//TODO: if you create a new customer instead of being existing customer, then u will update customer id in customer, but we need to update in all transactions also (need to automate).
//TODO: implement credit Report feature. (as a seperate menu)
//TODO: Deployment project.
//Add/Update Phone Number and address of customer.
//TODO: form for Config values.
//TODO: sometimes closed date is not updating.
// TODO: need to give popup for duplicate entry.









