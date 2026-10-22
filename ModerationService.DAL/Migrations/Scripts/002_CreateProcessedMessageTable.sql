create table if not exists processed_messages (
    message_id uuid primary key,
    processed_at timestamp with time zone not null default current_timestamp
);

create index if not exists ix_processed_messages_processed_at on processed_messages(processed_at);