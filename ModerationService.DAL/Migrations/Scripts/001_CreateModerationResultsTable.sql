create table if not exists moderation_results (
	id serial primary key,
	post_pending_id int not null,
	status int not null,
	rejection_reason text null,
	date_moderate timestamp without time zone not null default current_timestamp
);