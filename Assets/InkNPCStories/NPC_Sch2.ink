-> harvel_intro

=== harvel_intro ===
You approach a cluttered workbench where Harvel is examining a fragment of old Aurelian tech. 
He doesn’t look up.

HARVEL: Careful with your footing. One misstep and you’ll undo an hour of alignment work.

-> menu_stage1

=== menu_stage1 ===
+ [What are you working on?] YOU: What are you working on? -> ask_work
+ [Is that scrap really so sensitive?] YOU: Is that scrap really so sensitive? -> ask_fragile
+ [The scrap I've found looks like that, with those runes.] YOU: The scrap I've found looks like that, with those runes. -> ask_scrap
+ [Should I leave you alone?] YOU: Should I leave you alone? -> ask_leave

=== ask_work ===
HARVEL: These scrap pieces hold directive runes. Aurelian logic, etched directly into the material.
HARVEL: Most outsiders wouldn’t even notice them, much less understand their function.

-> menu_stage2

=== ask_fragile ===
HARVEL: Sensitive? Only because the Aurelians designed their tech to respond to even minor shifts.
HARVEL: Complexity isn’t fragility, though people often confuse the two.

-> menu_stage2

=== ask_scrap ===
HARVEL: Hm. At least you’re paying attention.
HARVEL: Yes, some of the fragments you’ve found carry similar structures. Some are simply remnants of ships carrying travellers just like you. 

-> menu_stage2

=== ask_leave ===
HARVEL: If you left every time you felt uncertain, we’d never get anywhere.

-> menu_stage2

=== menu_stage2 ===
HARVEL: Now that you're here, ask what you actually want to know.

+ [What do the runes do?] YOU: What do the runes do? -> ask_runes
+ [If I learned to translate these runes, could I use this scrap to repair my ship?] YOU: If I learned to translate these runes, could I use this scrap to repair my ship?  -> ask_repair
+ [How did you become the outpost’s mechanist?] YOU: How did you become the outpost's mechanist? -> ask_background
+ [That's all I needed.] YOU: That's all I needed. -> end_convo

=== ask_runes ===
HARVEL: They govern activation patterns, stability, and energy flow.
HARVEL: Think of them as instructions carved into the material itself.

-> menu_stage2

=== ask_repair ===
HARVEL: Well I don't know what state your ship is in or what tech you're working with.
HARVEL: But in theory, yes. If you grasped their structure, you could redirect their functions. Repairing your ship would be possible.

-> menu_stage2

=== ask_background ===
HARVEL: Years of study, precision, and patience. 

-> menu_stage2

=== end_convo ===

    -> END
