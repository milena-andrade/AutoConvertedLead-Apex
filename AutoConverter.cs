trigger AutoConverter on Lead (after insert) {
                        LeadStatus convertStatus = [
                            select MasterLabel
                            from LeadStatus
                            where IsConverted = true
                            limit 1
                        ];
                        List leadConverts = new List();

                        for (Lead lead: Trigger.new) {
                            if (!lead.isConverted && lead.WebForm__c == 'Free Trial') {
                                Database.LeadConvert lc = new Database.LeadConvert();
                                String oppName = lead.Name;

                                lc.setLeadId(lead.Id);
                                lc.setOpportunityName(oppName);
                                lc.setConvertedStatus(convertStatus.MasterLabel);

                                leadConverts.add(lc);
                            }
                        }

                        if (!leadConverts.isEmpty()) {
                            List lcr = Database.convertLead(leadConverts);
                        }
                    }
