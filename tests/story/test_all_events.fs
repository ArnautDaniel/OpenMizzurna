\ every converted event file loads, and the lookups by number and by address find their words
USING: tester events events.all ;

testing a pack's scripts by number
T{ 245 load-pack  0 pack-script  ' events.pack-049-0:script-0 = -> true }T

testing the main executable's scripts reach an area's words by address
T{ 0 event-area !  $80106048 overlay-word  ' events.ovl000:entry-80106048 = -> true }T

test-summary
