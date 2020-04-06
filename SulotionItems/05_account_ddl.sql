create table account_snapshots
(
account_id bigint,
etag integer,
"timestamp"  timestamp with time zone,
balance money
);
create index idx_account_snapshots_account_id on account_snapshots(account_id);


create table account_events
(
account_id bigint,
etag integer,
"timestamp"  timestamp with time zone,
transaction_type varchar(20),
amount money
);
create index idx_account_events_account_id on account_events(account_id);
