﻿using Shopway.Domain.Entities.Parents;

namespace Shopway.Domain.Entities;

public sealed class Project : WorkItem
{
    public virtual Employee? ProjectLeader { get; set; }
    public virtual int ProjectLeaderId { get; set; }
}